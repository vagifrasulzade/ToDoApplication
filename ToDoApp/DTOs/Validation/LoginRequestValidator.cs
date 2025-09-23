using FluentValidation;
using ToDoApp.DTOs.Auth;
namespace ToDoApp.DTOs.Validation;


public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();
        //RuleFor(x => x.Password)
        //.MinimumLength(8)
        //.Must(SaharedValidator.BeValidPassword)
        //.NotEmpty();

        RuleFor(x => x.Password)
            .Password()
            .NotEmpty();
    }
}
