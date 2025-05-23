using CashFlow.Commnication.Enums;
using CashFlow.Exception;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetById;

public class GetExpenseByIdTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";
    private readonly string _token;
    private readonly long _expenseId;

    public GetExpenseByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Member.GetToken();
        _expenseId = webApplicationFactory.Expense_Member.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().Should().Be(_expenseId);
        response.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
        response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);

        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();

        Enum.IsDefined(typeof(PaymentType), paymentType).Should().BeTrue();

    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var result = await DoGet(requestUri: $"{METHOD}/1000", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorsMessages.DESPESA_NAO_ENCONTRADA));

    }
}
