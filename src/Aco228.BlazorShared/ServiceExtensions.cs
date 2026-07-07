using Aco228.Common.Extensions;
using Aco228.Common.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Aco228.BlazorShared;

public static class ServiceExtensions
{
    public static void RegisterBlazorSharedServices(this IServiceCollection services)
        => typeof(ServiceExtensions).RegisterIfNot(() =>
        {
            services.RegisterServicesFromAssembly(typeof(ServiceExtensions).Assembly);
        });
}