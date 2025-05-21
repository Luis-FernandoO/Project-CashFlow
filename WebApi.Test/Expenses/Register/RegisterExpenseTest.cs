using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.Register;
public class RegisterExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";
    private readonly string _token;

    public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory): base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, request: request,token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);
        
        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;


        var result = await DoPost(requestUri: METHOD, request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorsMessages.TITULO_OBRIGATORIO));
    }

}
