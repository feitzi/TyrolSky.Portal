namespace TyrolSky.Portal {
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class Program {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config/appsettings.json", false, true)
            .AddJsonFile($"config/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .AddJsonFile($"config/appsettings.{Environment.MachineName}.json", true)
            .AddEnvironmentVariables()
            .Build();

        public static void Main(string[] args) {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            try {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            } catch (Exception ex) {
                Log.Fatal(ex, "Host terminated unexpectedly");
            } finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder => builder.AddConfiguration(Configuration))
                .UseSerilog() // Add this
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }

}