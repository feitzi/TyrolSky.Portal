using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace TyrolSky.Portal {
    using System;
    using System.ServiceProcess;
    using Configuration;
    using HealthCheck;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<SampleConfiguration>(Configuration.GetSection(SampleConfiguration.ConfigPath));

            services.AddHealthChecks()
                .AddDiskStorageHealthCheck(options => {
                    options.AddDrive("C:\\", 1000);
                })
                .AddWindowsServiceHealthCheck("TapiSrv", x => x.Status == ServiceControllerStatus.Running, null, "TapiService", null, new[] {
                    "Windows-Service",
                })
                .AddCheck<IsEvenMinuteHealthCheck>("SampleCheck", null, new[] {"SLA"})
                .AddCheck<LongRunningCheck>("LongRunning", HealthStatus.Degraded, new[] {"SLA"}, TimeSpan.FromSeconds(5))
                .AddUrlGroup(new Uri("https://localhost:5001/WeatherForecast"), "Weatherforecase endpoint");
            ;
            services.AddHealthChecksUI(settings => {
                    // Set the maximum history entries by endpoint that will be served by the UI api middleware
                    settings.MaximumHistoryEntriesPerEndpoint(50);
                })
                .AddInMemoryStorage();
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "TyrolSky.Portal", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TyrolSky.Portal v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthz", new HealthCheckOptions {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/healthSla", new HealthCheckOptions {
                    Predicate = check => check.Tags.Contains("SLA"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI(setupOptions => { });
            });
        }
    }

}