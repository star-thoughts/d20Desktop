using d20Web.Clients;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace d20Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddHttpClient<ICampaignServer, CampaignServer>(client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            });

            builder.Services.AddHttpClient<ICombatServer, CombatServer>(client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            });

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}