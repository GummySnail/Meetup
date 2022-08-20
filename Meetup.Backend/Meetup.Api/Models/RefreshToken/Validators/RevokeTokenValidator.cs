using FluentValidation;
using Meetup.Api.Models.RefreshToken.Requests;

namespace Meetup.Api.Models.RefreshToken.Validators;

public class RevokeTokenValidator : AbstractValidator<RevokeTokenRequest>
{
    public RevokeTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotNull().WithMessage("Refresh token cannot be null")
            .NotEmpty().WithMessage("Refresh token cannot be empty")
            .Length(44).WithMessage("Refresh token length must be 44");
    }
}