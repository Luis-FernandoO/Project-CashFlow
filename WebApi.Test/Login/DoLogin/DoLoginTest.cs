using CashFlow.Commnication.Requests;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.Login.DoLogin;
public class DoLoginTest : CashFlowClassFixture
{
    private const string METHOD = "api/Login";

    private readonly string _email;
    private readonly string _name;
    private readonly string _password;


    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _email = webApplicationFactory.User_Member.GetEmail();
        _name = webApplicationFactory.User_Member.GetName();
        _password = webApplicationFactory.User_Member.GetPassword();

    }
    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password =  _password
        };

        var response = await DoPost(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();

    }

    [Fact]
    public async Task Error_Login_Invalid()
    {
        var request = RequestLoginJsonBuilder.Build();
        
        var response = await DoPost(METHOD, request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);   
        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals("Email ou Senha Inválidos"));
    }
}
