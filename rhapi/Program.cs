using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using rhapi.Endpoints;
using rhapi.Poco;
using rhapi.Services;

namespace rhapi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.ConfigureServices();

            var app = builder.Build();
            app.ConfigureSwagger().UseHttpsRedirection().UseAuthorization();

            if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.MapGet("/", () => "This is my root page");
            app.MapAccountEndpoints();
            app.MapOrderEndpoints();
            app.MapMarketEndpoints();

            //app.MapExampleEndpoints();

            await app.RunAsync();
        }
    }

    internal static class StartupExtensions
    {
        internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var svc = builder.Services;

            svc.Configure<JsonOptions>(opt => 
            { 
                opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            });
            
            svc.AddAuthorization();

            // Configure crypto market service based on emulation settings
            var useEmulatedCryptoMarket = builder.Configuration.GetValue<bool>("Emulation:EmulateMarket");
            if (useEmulatedCryptoMarket)
            {
                Console.WriteLine("Emulating market...");
                svc.AddSingleton<IRhCryptoMarket, EmulatedRhCryptoMarket>();
            }
            else
            {
                svc.AddHttpClient<IRhCryptoMarket, RhCryptoMarket>(client => //this suffices for registering RhCryptoMarket - do not register separately
                {
                    //todo get both of these from appsettings.json. Move consts out of RhCryptoMarket.
                    client.Timeout = TimeSpan.FromSeconds(10);
                    client.BaseAddress = new Uri(RhCryptoMarket.BASE_URL);
                    client.DefaultRequestHeaders.Add(RhCryptoMarket.API_KEY_HEADER, Environment.GetEnvironmentVariable("rhak"));
                });
            }

            //svc.AddSingleton<ExampleVerify>();
            svc.AddSingleton<RhAccountEndpoints>();
            svc.AddSingleton<RhMarketEndpoints>();
            svc.AddSingleton<RhOrderEndpoints>();

            var settings = new AppSettings { IsDevelopment = builder.Environment.IsDevelopment() };
            svc.AddSingleton(settings);

            svc.AddEndpointsApiExplorer(); // required for swagger with minimal APIs
            svc.AddSwaggerGen();

            return builder;
        }

        internal static WebApplication ConfigureSwagger(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment()) return app;

            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
