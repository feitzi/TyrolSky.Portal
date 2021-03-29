namespace MediatorHandling.Ping {
    using System;
    using MediatR;

    public class Ping : IRequest<string> {

        public string PongValue { get; set; }
    }

}