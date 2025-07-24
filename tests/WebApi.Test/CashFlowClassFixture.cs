using MigraDoc.DocumentObjectModel;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class CashFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public CashFlowClassFixture(CustomWebApplicationFactory customWebApplicationFactory)
    {
        _httpClient = customWebApplicationFactory.CreateClient();
    }

    public async Task<HttpResponseMessage> DoPost(
        string requestUri,
        object request,
        string token = "",
        string culture = "pt-BR")
    {
        if (!token.IsValueNullOrEmpty()) AuthorizeRequest(token);

        if (!culture.IsValueNullOrEmpty()) ChangeRequestCulture(culture);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    protected async Task<HttpResponseMessage> DoGet(
        string requestUri,
        string token,
        string culture = "pt-BR")
    {
        if (!token.IsValueNullOrEmpty()) AuthorizeRequest(token);
        if (!culture.IsValueNullOrEmpty()) ChangeRequestCulture(culture);

        return await _httpClient.GetAsync(requestUri);
    }

    private void AuthorizeRequest(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}
