namespace TyrolSky.Portal {
    using System;
    using System.ServiceProcess;
    using Configuration;
    using HealthCheck;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using StackExchange.Redis.Extensions.Core.Configuration;
    using StackExchange.Redis.Extensions.Newtonsoft;

    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            ConfigRegistry.RegisterConfiguration(services, Configuration);

            services.AddHealthChecks().UseTyrolSkyChecks();

            services.AddHealthChecksUI(settings => {
                    // Set the maximum history entries by endpoint that will be served by the UI api middleware
                    settings.MaximumHistoryEntriesPerEndpoint(50);
                })
                .AddInMemoryStorage();
            services.AddControllers();
            var redisConfig = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfig);
            services.AddHealthChecks().AddRedis("localhost:6379");

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