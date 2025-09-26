using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UBSAg.Negotiations.Application.Behaviors;
using UBSAg.Negotiations.Application.Categories;
using UBSAg.Negotiations.Domain.Interfaces;

namespace UBSAg.Negotiations.Application
{
    public static class ConfigurationDI
    {
        public static void AddApplicationLayer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<LowRisk>();
            serviceCollection.AddTransient<MediumRisk>();
            serviceCollection.AddTransient<HighRisk>();
            serviceCollection.AddTransient<NoCategory>();

            serviceCollection.AddTransient<ICategorizerTrades, CategorizerTrades>(provider =>
            {
                var lowRisk = provider.GetRequiredService<LowRisk>();
                var mediumRisk = provider.GetRequiredService<MediumRisk>();
                var highRisk = provider.GetRequiredService<HighRisk>();
                var noCategory = provider.GetRequiredService<NoCategory>();

                lowRisk.SetNext(mediumRisk);
                mediumRisk.SetNext(highRisk);
                highRisk.SetNext(noCategory);

                return new CategorizerTrades(lowRisk);
            });

            
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));            
            serviceCollection.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(ConfigurationDI).Assembly);
                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            serviceCollection.AddValidatorsFromAssembly(typeof(ConfigurationDI).Assembly, includeInternalTypes: true);

        }
    }
}
