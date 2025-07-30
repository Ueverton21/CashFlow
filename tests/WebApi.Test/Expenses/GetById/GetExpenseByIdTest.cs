using CashFlow.Communication.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Test.Expenses.GetById
{
    public class GetExpenseByIdTest : CashFlowClassFixture
    {
        private const string METHOD = "api/Expenses";
        private readonly string _token;
        private readonly long _expenseId;

        public GetExpenseByIdTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
        {
            _token = customWebApplicationFactory.User_Team_Member.GetToken();
            _expenseId = customWebApplicationFactory.Expense_MemberTeam.GetExpenseId();
        }
        [Fact]
        public async Task Success()
        {
            var result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);
         
            response.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
            response.RootElement.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
            response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
            response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);

            var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
            Enum.IsDefined(typeof(PaymentType), paymentType).Should().BeTrue();
        }
    }
}
