using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Config;
using DanceCompetitionHelper.Database.Diagnostic;
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

                // Add services to the container.
                builder.Services
                    .AddDbContext<DanceCompetitionHelperDbContext>()
                    .AddScoped<IDanceCompetitionHelper, DanceCompetitionHelper>()
                    .AddScoped<IObserver<DiagnosticListener>, DbDiagnosticObserver>()
                    .AddScoped<IObserver<KeyValuePair<string, object?>>, DbKeyValueObserver>()
                    .AddSingleton<IDbConfig>(myCfg)
                    .AddControllersWithViews();

                builder.Host.UseNLog();

                var app = builder.Build();

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

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Competition}/{action=Index}/{competitionId?}");

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