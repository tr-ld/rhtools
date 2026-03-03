using abstractions.Repositories;
using abstractions.Services;
using emulation.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RHWebFront.Components;
using RHWebFront.Config;
using RHWebFront.Data;
using RHWebFront.Data.Migration;
using RHWebFront.Repositories;
using RHWebFront.Services;
using RHWebFront.Services.RuleDisplay;

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

            // Ensure database is created and migrations are applied (only when not using emulation)
            using (var scope = app.Services.CreateScope())
            {
                var emulationConfig = scope.ServiceProvider.GetRequiredService<IOptions<EmulationConfig>>().Value;
                
                if (!emulationConfig.EnableRepositoryEmulation)
                {
                    //todo: move db location to appdata
                    var db = scope.ServiceProvider.GetRequiredService<RhDbContext>();
                    db.Database.Migrate();
                }
            }

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
        extension(WebApplicationBuilder builder)
        {
            internal WebApplicationBuilder ConfigureServices()
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

                // Database
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=rhtools.db";
                svc.AddDbContextPool<RhDbContext>(options => 
                    options.UseSqlite(connectionString).ReplaceService<IMigrationsSqlGenerator, RhToolsSqliteMigrationsSqlGenerator>());

                // Load config early to determine which repositories to register
                builder.LoadConfig();
                var emulationConfig = builder.Configuration.GetSection("EmulationConfig").Get<EmulationConfig>() ?? new EmulationConfig();

                // Conditionally register repositories based on emulation setting
                if (emulationConfig.EnableRepositoryEmulation)
                {
                    svc.AddSingleton<ISymbolWatchlistRepository, EmulatedSymbolWatchlistRepository>();
                    svc.AddSingleton<IBidAskHistoryRepository, EmulatedBidAskHistoryRepository>();
                    svc.AddSingleton<IRuleRepository, EmulatedRuleRepository>();
                }
                else
                {
                    svc.AddScoped<ISymbolWatchlistRepository, SymbolWatchlistRepository>();
                    svc.AddScoped<IBidAskHistoryRepository, BidAskHistoryRepository>();
                    svc.AddScoped<IRuleRepository, RuleRepository>();
                }

                // HTTP Client & Asset Manager
                svc.AddHttpClient<IRhApiClient, RhApiClient>(client => //this suffices for registering RhApiClient - do not register separately
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    client.BaseAddress = new Uri(RhApiClient.BASE_URL);
                });
                svc.AddScoped<IRhAssetManager, RhAssetManager>();
                svc.AddScoped<IRuleManager, RuleManager>();

                // Singleton Services
                svc.AddSingleton<ISymbolWatchlistService, SymbolWatchlistService>();
                svc.AddSingleton<IBidAskNotificationService, BidAskNotificationService>();
                svc.AddScoped<ISelectedSymbolManager, SelectedSymbolManager>();

                // Background Service
                svc.AddHostedService<BidAskPollingService>();
                svc.AddScoped<RuleEditorStateService>();
                svc.AddScoped<RuleDisplayCompositor>();

                return builder;
            }

            private void LoadConfig()
            {
                builder.Services.AddOptions<AppConfig>()
                    .Bind(builder.Configuration.GetSection("AppConfig"))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
                    
                builder.Services.AddOptions<RulesConfig>()
                    .Bind(builder.Configuration.GetSection("Rules"))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
                    
                builder.Services.AddOptions<CacheConfig>()
                    .Bind(builder.Configuration.GetSection("CacheConfig"))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
                    
                builder.Services.AddOptions<EmulationConfig>()
                    .Bind(builder.Configuration.GetSection("EmulationConfig"))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
            }
        }
    }
}
