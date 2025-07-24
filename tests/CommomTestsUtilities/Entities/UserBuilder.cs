using Bogus;
using CashFlow.Domain.Entities;
using CommomTestsUtilities.Cryptography;

namespace CommomTestsUtilities.Entities;

public class UserBuilder
{
    public static User Build() {

        var passwordEncripter = new PasswordEncrypterBuilder().Build();

        return new Faker<User>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Name, faker => faker.Person.FullName)
            .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Email))
            .RuleFor(u => u.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
            .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid());
    }
}
