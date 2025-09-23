using OrderGenerator.Application.UseCases;
using OrderGenerator.Application.UseCases.Interfaces;
using OrderGenerator.Application.UseCases.Interfaces.Interfaces;

namespace OrderGenerator.Application.DependencyInjections;

public static class ApplicationDependencies
{
    public static void AddApplicationDepedencies(this IServiceCollection services)
    {
        services.AddScoped<INewOrderUseCase, NewOrderUseCase>();
    }
}