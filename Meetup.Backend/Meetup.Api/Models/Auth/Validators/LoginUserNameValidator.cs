using FluentValidation;
using Meetup.Api.Models.Auth.Requests;

namespace Meetup.Api.Models.Auth.Validators;

public class LoginUserNameValidator : AbstractValidator<LoginUserNameRequest>
{
    public LoginUserNameValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull().WithMessage("Username cannot be null")
            .NotEmpty().WithMessage("Username cannot be empty")
            .MinimumLength(3).WithMessage("Minimum username length must be from 3 symbols")
            .MaximumLength(24).WithMessage("Maximum username length must be less than 24 symbols")
            .Matches("^[^.](.*[^.])?$").WithMessage("Username cannot start and end from symbol '.'")
            .Matches("^[^0-9]").WithMessage("First symbol cannot be a number")
            .Matches("^[a-zA-Z0-9_.]+$").WithMessage("Username can only contains latin characters, numbers, symbole '_' and '.'");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("Password cannot be null")
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("Minimum password length must be from 8 symbols")
            .MaximumLength(32).WithMessage("Maximum password length must be less than 32 symbols")
            .Matches("[A-Z]").WithMessage("Password must contain at least one or more capital letters")
            .Matches("[0-9]").WithMessage("Password must contain at least one or more numbers")
            .Matches(@"[][""!@#$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("Password must contain one or more special characters");
    }
}