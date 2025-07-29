using CommomTestsUtilities.Requests;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using FluentAssertions;
using System.Net.Http.Headers;
using WebApi.Test.InlineData;
using CashFlow.Exception;
using System.Globalization;

namespace WebApi.Test.Expenses.Register;

public class RegisterExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";

    private readonly string _token;

    public RegisterExpenseTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _token = customWebApplicationFactory.User_Team_Member.GetToken();
    }
    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPost(METHOD, request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Empty(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = "";

        var result = await DoPost(METHOD, request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessage").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

}
