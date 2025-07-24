using CashFlow.Communication.Requets;
using CashFlow.Exception;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace CashFlow.Application.UseCases.Users;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "PasswordValidator";
    private const string ERROR_MESSAGE_KEY = "ErrorMessage";

    
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password) || 
            password.Length < 8 || 
            Regex.IsMatch(password,@"[A-Z]+") is false ||
            Regex.IsMatch(password, @"[a-z]+") is false ||
            Regex.IsMatch(password, @"[0-9]+") is false ||
            Regex.IsMatch(password, @"[\!\?\*\.\@]+") is false)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.PASSWORD_INVALID);
            return false;
        }
        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ERROR_MESSAGE_KEY}}}";
    }
}