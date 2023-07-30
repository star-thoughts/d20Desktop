using d20Web.Hubs;
using d20Web.Identity;
using d20Web.Services;
using d20Web.Storage;
using d20Web.Storage.MongoDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Data;

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