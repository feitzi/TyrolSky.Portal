namespace MediatorHandling.Ping {
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    internal class PingHandler : IRequestHandler<Ping, string> {
        public Task<string> Handle(Ping request, CancellationToken cancellationToken) {
            return Task.FromResult(request.PongValue);
        }

      
    }
}