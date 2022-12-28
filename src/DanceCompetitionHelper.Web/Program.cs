using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Config;
using DanceCompetitionHelper.Database.Diagnostic;
using System.Diagnostics;

namespace DanceCompetitionHelper.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
                .AddDbContext<DanceCompetitionHelperDbContext>()
                .AddSingleton<IDanceCompetitionHelper, DanceCompetitionHelper>()
                .AddSingleton<IObserver<DiagnosticListener>, DbDiagnosticObserver>()
                .AddSingleton<IObserver<KeyValuePair<string, object?>>, DbKeyValueObserver>()
                // GO ON WITH CONFIG!..
                .AddSingleton<IDbConfig>(
                    builder.Configuration
                        // .GetSection(SqLiteDbConfig.Name)
                        .GetValue<IDbConfig>(SqLiteDbConfig.Name))
                .AddControllersWithViews();
            builder.Services
                .Configure<IDbConfig>(
                    builder.Configuration
                        .GetSection(SqLiteDbConfig.Name));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}