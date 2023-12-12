using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;

namespace GeneralSecureApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //ローカライズの設定
            builder.Services.AddLocalization();
            var host = builder.Build();
            var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
            var result = await jsRuntime.InvokeAsync<string>("blazorCulture.get");
            var culture = CultureInfo.GetCultureInfo(result);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            builder.Services.AddHttpClient("GeneralSecureApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
                
            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GeneralSecureApp.ServerAPI"));

            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
    }
}
