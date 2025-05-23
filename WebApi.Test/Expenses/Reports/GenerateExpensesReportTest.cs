using FluentAssertions;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Expenses.Reports;
public class GenerateExpensesReportTest : CashFlowClassFixture
{
    private const string METHOD = "api/Report";
    
    private readonly string _adminToken;
    private readonly string _memberToken;
    private readonly DateTime _expenseDate;

    public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.User_Admin.GetToken();
        _memberToken= webApplicationFactory.User_Member.GetToken();
        _expenseDate= webApplicationFactory.Expense_Admin.GetDate();
    }


    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_expenseDate:Y}", token: _memberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_expenseDate:Y}", token: _memberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
