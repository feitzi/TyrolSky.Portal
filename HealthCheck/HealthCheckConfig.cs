using System;

namespace HealthCheck {
    using HealthCheck;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public static class HealthCheckConfig {

        public static void UseTyrolSkyChecks(this IHealthChecksBuilder healthChecksBuilder) {
            healthChecksBuilder.AddCheck<LongRunningCheck>("LongRunning", HealthStatus.Degraded, new[] {"SLA"}, TimeSpan.FromSeconds(5));
        }
    }
}