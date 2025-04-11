using Microsoft.Extensions.DependencyInjection;
using Services.Crud.Base;
using Services.DbTransactions.Abstracts;
using Services.DbTransactions;
using Services.User.Provider;
using Services.User.SystemScopeAccessor;

namespace Services;

public static partial class Initializer
{
    private static IServiceCollection AddServiceInjection(this IServiceCollection services)
    {
        services.AddScoped<ISystemUserScopeAccessor, SystemUserScopeAccessor>();
        services.AddScoped<ITransactionScopeFactory, TransactionScopeFactory>();
        services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));

        services.AddScoped<IUserProvider, UserProvider>();

        return services;
    }
}