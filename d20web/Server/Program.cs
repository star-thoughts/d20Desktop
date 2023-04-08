using d20Web.Services;
using d20Web.Storage;
using d20Web.Storage.MongoDB;
using Microsoft.Extensions.Options;

namespace d20web
{
    public class Program
    {
        public static void Main(string[] args)
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
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}