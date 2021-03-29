using Microsoft.Extensions.DependencyInjection;

namespace MediatorHandling {
    using MediatR;

    public static class MediatorHandlingRegistration {

        public static void RegisterMediatorHandling(this IServiceCollection serviceCollection) {
            serviceCollection.AddMediatR(typeof(MediatorHandlingRegistration));
        }
    }
}