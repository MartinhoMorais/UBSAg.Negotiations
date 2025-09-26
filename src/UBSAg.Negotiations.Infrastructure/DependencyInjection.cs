using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UBSAg.Negotiations.Domain.Repositories;
using UBSAg.Negotiations.Infrastructure.Persistence;

namespace UBSAg.Negotiations.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            AddPersistence(services, configuration);

            return services;
        }

        private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("DefaultConnection");
            services.AddSingleton(new DapperContext(connString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITradeRepository, TradeRepository>();
        }

    }
}
