using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Infrastructure.DataAcess;
using CashFlow.Infrastructure.DataAcess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

public static class DependencyInjetionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IExpensesReadOnlyRepository, ExpenseRepository>();
        services.AddScoped<IExpensesWriteOnlyRepository, ExpenseRepository>();
        services.AddScoped<IExpensesUpdateOnlyRepository, ExpenseRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var version = new Version(8, 0, 37);
        var serverVersion = new MySqlServerVersion(version);    
        services.AddDbContext<CashFlowDbContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion);
        });
    }
}
