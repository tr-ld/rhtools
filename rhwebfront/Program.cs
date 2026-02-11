using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RHWebFront.Components;
using RHWebFront.Services;

namespace RHWebFront
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.ConfigureServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }

    internal static class StartupExtensions 
    {
        internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var svc = builder.Services;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() },
                NullValueHandling = NullValueHandling.Ignore,
                Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
            };

            // Add services to the container.
            svc.AddRazorComponents()
                   .AddInteractiveServerComponents();

            svc.AddMemoryCache();

            svc.AddHttpClient<IRhApiClient, RhApiClient>(client => //this suffices for registering RhApiClient - do not register separately
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                client.BaseAddress = new Uri(RhApiClient.BASE_URL);
            });

            svc.AddScoped<IRhAssetManager, RhAssetManager>();

            return builder;
        }
    }
}
