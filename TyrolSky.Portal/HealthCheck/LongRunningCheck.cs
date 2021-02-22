namespace TyrolSky.Portal.HealthCheck {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Logging;

    public class LongRunningCheck : IHealthCheck {
        public ILogger<LongRunningCheck> Logger { get; }

        public LongRunningCheck(ILogger<LongRunningCheck> logger) {
            Logger = logger;
            Logger.LogInformation("HC Service started!");
        }

        private static HealthCheckResult LastResult = HealthCheckResult.Degraded("Check was not executed!");
        private static readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) {
            // do not wait!
            var ignoredTask = Task.Run(() => CheckHealthAsyncInternal(context));
            return LastResult;
        }

        private async Task CheckHealthAsyncInternal(HealthCheckContext context) {
            await _mutex.WaitAsync(new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);

            Logger.LogInformation("Start Delay!");
            await Task.Delay(TimeSpan.FromSeconds(35));
            Logger.LogInformation("Delay finished!");
            LastResult = new HealthCheckResult(HealthStatus.Healthy, $"This was finished at {DateTime.UtcNow}");

            _mutex.Release();
        }
    }
}