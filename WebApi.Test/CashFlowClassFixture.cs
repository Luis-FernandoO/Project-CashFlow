using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;
public class CashFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public CashFlowClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    protected async Task DoPost(string requestUri, object request, string? token = "")
    {
        AuthorizeRequest(token);
        var result = await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return;
        }
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
