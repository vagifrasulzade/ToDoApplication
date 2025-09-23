using FluentValidation;
using System.Text.RegularExpressions;
using ToDoApp.DTOs.Auth;

namespace ToDoApp.DTOs.Validation;
/// <summary>
/// 
/// </summary>
public class RegisterRequestValidotor:AbstractValidator<RegisterRequest>
{

    /// <summary>
    /// 
    /// </summary>
   public RegisterRequestValidotor()
   {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();
        //RuleFor(x => x.Password)
        //    .Must(BeValidPassword)
        //    .MinimumLength(8)
        //    .NotEmpty();

        //RuleFor(x => x.Password)
        //    .MinimumLength(8)
        //    .Must(SaharedValidator.BeValidPassword)
        //    .NotEmpty();

        RuleFor(x=>x.Password)
            .Password(mustContainsDigit:false)
            .MinimumLength(8)
            .NotEmpty();


    }
    //private bool BeValidPassword(string password)
    //{
    //    return new Regex(@"\d").IsMatch(password)
    //        && new Regex(@"[a-z]").IsMatch(password)
    //        && new Regex(@"[A-z]").IsMatch(password);
    //}
}
