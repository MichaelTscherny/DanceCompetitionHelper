using DanceCompetitionHelper.Config;
using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Config;
using DanceCompetitionHelper.Database.Diagnostic;
using DanceCompetitionHelper.Helper;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using DanceCompetitionHelper.Web.Controllers;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using NLog.Web;
using System.Diagnostics;

namespace DanceCompetitionHelper.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // that's on purpose for easier testing
                SqLiteDbConfig myCfg = new SqLiteDbConfig();
                builder.Configuration
                    .GetRequiredSection(
                        SqLiteDbConfig.Name)
                    .Bind(myCfg);
                ImporterSettings myImporterSettigns = new ImporterSettings();
                builder.Configuration
                    .GetSection(
                        ImporterSettings.Name)
                    ?.Bind(myImporterSettigns);

                // Add services to the container.
                builder.Services
                    .AddDbContext<DanceCompetitionHelperDbContext>()
                    .AddScoped<IDanceCompetitionHelper, DanceCompetitionHelper>()
                    .AddSingleton<IObserver<DiagnosticListener>, DbDiagnosticObserver>()
                    .AddSingleton<IObserver<KeyValuePair<string, object?>>, DbKeyValueObserver>()
                    // general settings
                    .AddSingleton<IDbConfig>(myCfg)
                    .AddSingleton(myImporterSettigns)
                    // general classes
                    .AddTransient<TableHistoryCreator>()
                    .AddControllersWithViews();

                // Organization specific classes:
                // Oetsv:
                builder.Services
                    .AddTransient<OetsvCompetitionImporter>()
                    .AddTransient<OetsvParticipantChecker>();

                builder.Services
                    .AddDistributedMemoryCache()
                    .AddSession(options =>
                    {
                        options.IdleTimeout = TimeSpan.FromHours(10);
                        options.Cookie.HttpOnly = true;
                        options.Cookie.IsEssential = true;
                    });

                builder.Host.UseNLog();

                var app = builder.Build();

                var logger = app.Services
                    .GetRequiredService<ILogger<CompetitionController>>();

                // for sql-tracing/loggings...
                DiagnosticListener.AllListeners.Subscribe(
                    app.Services
                        .GetRequiredService<IObserver<DiagnosticListener>>());

                logger.LogInformation(
                    "Working in '{EnvironmentName}'",
                    app.Environment.EnvironmentName);

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment() == false)
                {
                    app.UseExceptionHandler("/Home/Error");

                    app.MapFallbackToController(
                        "Index",
                        "Competition");
                }

                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthorization();

                app.UseSession();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Competition}/{action=Index}/{id?}");

                app.Services
                    .GetRequiredService<IHostApplicationLifetime>()
                    .ApplicationStarted.Register(
                        () =>
                        {
                            var addresses = app.Services.
                                GetRequiredService<IServer>()
                                ?.Features
                                ?.Get<IServerAddressesFeature>()
                                ?.Addresses ?? new List<string>();

                            logger.LogInformation(
                                "Reachable at: {addresses}",
                                string.Join(
                                    ", ",
                                    addresses));

                            if (app.Environment.IsDevelopment() == false
                                || Debugger.IsAttached == false)
                            {
                                foreach (var curAddr in addresses)
                                {
                                    if (Uri.TryCreate(
                                        curAddr,
                                        UriKind.Absolute,
                                        out _) == false)
                                    {
                                        continue;
                                    }

                                    try
                                    {
                                        Process.Start(
                                            new ProcessStartInfo()
                                            {
                                                FileName = curAddr,
                                                UseShellExecute = true,

                                            });

                                        return;
                                    }
                                    catch (System.ComponentModel.Win32Exception noBrowser)
                                    {
                                        if (noBrowser.ErrorCode == -2147467259)
                                        {
                                            logger.LogWarning(
                                                noBrowser,
                                                "No browser found!");
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        logger.LogError(
                                            exc,
                                            "Unable to start/open '{address}'",
                                            curAddr);
                                    }
                                }
                            }
                        });

                app.Run();
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads
                // before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }
}