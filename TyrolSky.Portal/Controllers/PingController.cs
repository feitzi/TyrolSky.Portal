namespace TyrolSky.Portal.Controllers {
    using System.Threading.Tasks;
    using MediatorHandling.Ping;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class PingController : Controller {
        private ILogger<PingController> Logger { get; }
        private IMediator Mediator { get; }

        public PingController(ILogger<PingController> logger, IMediator mediator) {
            Logger = logger;
            Mediator = mediator;
        }

        [HttpGet]
        public async Task<string> Ping(string value) {
            string result = await Mediator.Send(new Ping {PongValue = "SamplePong"});
            return result;
        }

    }
}