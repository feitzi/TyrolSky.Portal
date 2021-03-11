namespace TyrolSky.Portal.Configuration {
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ConfigRegistry {
        public static void RegisterConfiguration(IServiceCollection services, IConfiguration configuration) {
            services.AddOptions<SampleConfiguration>().Bind(configuration.GetSection(SampleConfiguration.ConfigPath));
            // .ValidateDataAnnotations();
        }
    }
}