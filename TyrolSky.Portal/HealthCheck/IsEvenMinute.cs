namespace TyrolSky.Portal.HealthCheck {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public class IsEvenMinuteHealthCheck : IHealthCheck {

        public async Task<HealthCheckResult>  CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken()) {
            var currentMinute = DateTime.Now.Minute;
            if (currentMinute % 2 ==0) {
            return HealthCheckResult.Healthy($"The current minute {currentMinute} is even!");
            }

            return HealthCheckResult.Unhealthy($"The current minute {currentMinute} is not even!");

        }
    }
}