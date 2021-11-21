using Microsoft.Extensions.DependencyInjection;
using MyVoby.Banking.Data.Context;
using MyVoby.Banking.Application;
using MyVoby.Banking.Data.Repository;
using MyVoby.Banking.Domain.Interfaces;
using MyVoby.Banking.Domain.Interfaces.Services;
using MyVoby.Domain.Core.Bus;
using MyVoby.Infra.Bus;

namespace MyVoby.Infra.IoC;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        //domain bus
        services.AddTransient<IEventBus, RabbitMQBus>();
        
        //Application services
        services.AddTransient<IAccountService, AccountService>();

        //Data
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<BankingDbContext>();
    }
}