using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.Update;

public class UpdateExpenseTest : CashFlowClassFixture
{
    private readonly String METHOD = "api/Expenses";

    private readonly string _token;
    private readonly long _expenseId ;

    public UpdateExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{
        _token = webApplicationFactory.User_Member.GetToken();
        _expenseId = webApplicationFactory.Expense_Member.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;
        var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorsMessages.TITULO_OBRIGATORIO));
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {

        var request = RequestExpenseJsonBuilder.Build();
        var result = await DoPut(requestUri: $"{METHOD}/0", request: request, token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorsMessages.DESPESA_NAO_ENCONTRADA));
    }

}
