using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommomTestsUtilities.Entities;
using CommomTestsUtilities.LoggedUser;
using CommomTestsUtilities.Mapper;
using CommomTestsUtilities.Repositories;
using CommomTestsUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.Update;

public class UpdateExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";

    private readonly string _token;
    private readonly long _expenseId;
    public UpdateExpenseTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _token = customWebApplicationFactory.User_Team_Member.GetToken();
        _expenseId = customWebApplicationFactory.Expense_MemberTeam.GetExpenseId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token);
        
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    //[Fact]
    //public async Task Error_Title_Empty()
    //{
    //    var request = RequestExpenseJsonBuilder.Build();
    //    request.Title = string.Empty;

    //    var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token);
    //    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    //    var body = await result.Content.ReadAsStreamAsync();
    //    var response = await JsonDocument.ParseAsync(body);
    //    var errors = response.RootElement.GetProperty("errorMessage").EnumerateArray();
    //    var expectedMessage = ResourceErrorMessages.TITLE_REQUIRED;

    //    errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    //}
    //[Fact]
    //public async Task Error_Expense_Not_Found()
    //{
    //    var request = RequestExpenseJsonBuilder.Build();

    //    var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token);
    //    result.StatusCode.Should().Be(HttpStatusCode.NotFound);

    //    var body = await result.Content.ReadAsStreamAsync();
    //    var response = await JsonDocument.ParseAsync(body);
    //    var errors = response.RootElement.GetProperty("errorMessage").EnumerateArray();
    //    var expectedMessage = ResourceErrorMessages.EXPENSE_NOT_FOUND;

    //    errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    //}
}
