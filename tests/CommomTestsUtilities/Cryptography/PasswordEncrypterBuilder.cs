using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommomTestsUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncripter> _mock;

    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncripter>();
        _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("!Val51dek");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if (string.IsNullOrWhiteSpace(password) is false)
        {
            _mock.Setup(passwordEncrypter => passwordEncrypter.VerifyPassword(password, It.IsAny<string>())).Returns(true);
        }
        
        return this;
    }
    public IPasswordEncripter Build()
    {
        return _mock.Object;
    }
}
