using Bogus;
using CashFlow.Communication.Requets;

namespace CommomTestsUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(r => r.Email, faker => faker.Person.Email)
            .RuleFor(u => u.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
