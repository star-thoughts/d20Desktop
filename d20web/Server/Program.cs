using AspNetCore.Identity.Mongo;
using d20Web.Hubs;
using d20Web.Identity;
using d20Web.Services;
using d20Web.Storage;
using d20Web.Storage.MongoDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace d20web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();

            StorageSettings storageSettings = new StorageSettings();
            builder.Configuration.Bind("Storage", storageSettings);
            builder.Services.AddSingleton(storageSettings);

            switch (storageSettings.Type)
            {
                case StorageType.MongoDB:
                    builder.Services.AddSingleton<ICombatStorage, CombatStorage>();
                    builder.Services.AddSingleton<ICampaignStorage, CampaignStorage>();
                    break;
            }

            ConfigureAuth(builder);

            builder.Services.AddTransient<ICampaignsService, CampaignsService>();
            builder.Services.AddTransient<ICombatService, CombatService>();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapRazorPages();
            app.MapControllers();
            app.MapHub<CampaignHub>("/hub/campaign");
            app.MapFallbackToFile("index.html");

            await ConfigureAuth(app);

            await app.RunAsync();
        }

        private static void ConfigureAuth(WebApplicationBuilder builder)
        {
            JwtConfiguration jwtConfig = new JwtConfiguration();
            builder.Configuration.Bind("Jwt", jwtConfig);

            if (string.IsNullOrWhiteSpace(jwtConfig.Key))
                throw new ArgumentNullException(nameof(jwtConfig.Key));
            if (string.IsNullOrWhiteSpace(jwtConfig.Issuer))
                throw new ArgumentNullException(nameof(jwtConfig.Issuer));

            builder.Services.Configure<JwtConfiguration>(options =>
            {
                options.Issuer = jwtConfig.Issuer;
                options.Key = jwtConfig.Key;
                options.ExpireDays = jwtConfig.ExpireDays;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Issuer,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                    config.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            StringValues accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            PathString path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            StorageSettings identityStorage = new StorageSettings();
            builder.Configuration.Bind("Identity", identityStorage);

            builder.Services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireNonAlphanumeric = true;
                identityOptions.Password.RequireDigit = true;
                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
            },
            databaseOptions =>
            {
                databaseOptions.ConnectionString = identityStorage.ConnectionString;
                databaseOptions.MigrationCollection = "d20_migration";
                databaseOptions.RolesCollection = "d20_roles";
                databaseOptions.UsersCollection = "d20_users";
            });

            IdentityDefaults identityDefaults = new IdentityDefaults();
            builder.Configuration.Bind("IdentityDefaults", identityDefaults);
            builder.Services.AddSingleton(identityDefaults);

            //  This seems to be required to prevent ASP.NET Core from redirecting to another page when a 401 is returned
            builder.Services.ConfigureApplicationCookie(p =>
            {
                p.Events.OnRedirectToAccessDenied =
                p.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
        }

        private static async Task ConfigureAuth(IHost host)
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                RoleManager<ApplicationRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                string[] existingRoles = roleManager.Roles.Select(p => p.Name).OfType<string>().ToArray();
                string[] allRoles = Roles.GetAllRoles().ToArray();
                string[] rolesToAdd = allRoles.Except(existingRoles).ToArray();

                //  Add any new roles to the system
                foreach (string role in rolesToAdd)
                    await roleManager.CreateAsync(new ApplicationRole() { Name = role });

                if (!userManager.Users.Any())
                {
                    IdentityDefaults identityDefaults = scope.ServiceProvider.GetRequiredService<IdentityDefaults>();
                    if (string.IsNullOrWhiteSpace(identityDefaults.AdminAccount)
                        || string.IsNullOrWhiteSpace(identityDefaults.AdminPassword))
                        throw new InvalidOperationException("Cannot start service without users defined.");

                    IdentityResult createResult = await userManager.CreateAsync(new ApplicationUser() { UserName = identityDefaults.AdminAccount, IsVerified = true, IsSiteAdmin = true }, identityDefaults.AdminPassword);
                    if (!createResult.Succeeded)
                        throw new InvalidOperationException("Could not create default admin account");

                    ApplicationUser? admin = userManager.Users.FirstOrDefault(p => p.UserName == identityDefaults.AdminAccount);
                    if (admin == null)
                        throw new InvalidOperationException("Cannot configure service without an administrator.");

                    await userManager.AddToRolesAsync(admin, Roles.GetSiteAdminRoles());
                }
            }
        }
    }
}