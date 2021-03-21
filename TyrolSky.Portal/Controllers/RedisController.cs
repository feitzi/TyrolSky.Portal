using Microsoft.AspNetCore.Mvc;

namespace TyrolSky.Portal.Controllers {
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using StackExchange.Redis;
    using StackExchange.Redis.Extensions.Core.Abstractions;

    [ApiController]
    [Route("[controller]")]
    public class RedisController : Controller {
        private ILogger<RedisController> Logger { get; }
        private IRedisCacheClient RedisCacheClient { get; }

        public RedisController(ILogger<RedisController> logger, IRedisCacheClient redisCacheClient) {
            Logger = logger;
            RedisCacheClient = redisCacheClient;
        }

        [HttpGet("write/{value}")]
        public void Write(string value) {
            Logger.LogInformation("Start to write to redis {@Value}", value);
            RedisCacheClient.GetDbFromConfiguration().AddAsync("myName", value, TimeSpan.FromMinutes(10), flag: CommandFlags.FireAndForget);
        }

        [HttpGet("read")]
        public async Task<string> Read() {
            Logger.LogInformation("Start to read from redis");

            string result = await RedisCacheClient.GetDbFromConfiguration().GetAsync<string>("myName");
            Logger.LogInformation("Result: {@RedisResult}", result);
            

            return result ?? "Not found";
        }

        [HttpGet("status")]
        public async Task<Dictionary<string, string>> Status() {
            return await RedisCacheClient.GetDbFromConfiguration().GetInfoAsync();
        }
    }
}