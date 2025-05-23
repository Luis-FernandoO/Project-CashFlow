using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAcess;
using CommonTestUtilities.Entities;
using DocumentFormat.OpenXml.Office2019.Drawing.Ink;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager Expense_Admin { get; private set; } = default!;
    public ExpenseIdentityManager Expense_Member { get; private set; } = default!;
    public UserIdentityManager User_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<CashFlowDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });
                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var accesTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccesTokenGenerator>();

                StartDataBase(dbContext, passwordEncripter, accesTokenGenerator);
            });
    }
    

    private void StartDataBase(CashFlowDbContext context, IPasswordEncripter passwordEncripter, IAccesTokenGenerator accesTokenGenerator)
    {
        var userTeamMember = AddUsersMember(context, passwordEncripter, accesTokenGenerator);
        var expenseMemberTeam = AddExpenses(context, userTeamMember,expenseId: 1);
        Expense_Member = new ExpenseIdentityManager(expenseMemberTeam);

        var userAdmin = AddUserAdmin(context, passwordEncripter, accesTokenGenerator);
        var expenseAdminTeam = AddExpenses(context, userAdmin, expenseId: 2);
        Expense_Admin = new ExpenseIdentityManager(expenseAdminTeam);

        context.SaveChanges();
    }

    private User AddUsersMember(CashFlowDbContext context,
        IPasswordEncripter passwordEncripter, 
        IAccesTokenGenerator accesTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;
         
        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);
        context.Users.Add(user);

        
        var token = accesTokenGenerator.Generate(user);

        User_Member = new UserIdentityManager(user, token, password);
        
        return user;
    }

    private User AddUserAdmin(CashFlowDbContext context,
        IPasswordEncripter passwordEncripter,
        IAccesTokenGenerator accesTokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;
        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);
        context.Users.Add(user);


        var token = accesTokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, token, password);

        return user;
    }

    private Expense AddExpenses(CashFlowDbContext context, User user,long expenseId)
    {
       var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;
        context.Expenses.Add(expense);

        return expense;
    }
}
