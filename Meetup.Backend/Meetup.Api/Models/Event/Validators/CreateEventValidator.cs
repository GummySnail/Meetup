using FluentValidation;
using Meetup.Api.Models.Event.Requests;

namespace Meetup.Api.Models.Event.Validators;

public class CreateEventValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty")
            .MinimumLength(3).WithMessage("Minimum name length must be from 3 symbols")
            .MaximumLength(20).WithMessage("Maximum name length must be less than 20 symbols");
        
        RuleFor(x => x.Description)
            .NotNull().WithMessage("Description cannot be null")
            .NotEmpty().WithMessage("Description cannot be empty")
            .MinimumLength(5).WithMessage("Minimum description length must be from 5 symbols")
            .MaximumLength(150).WithMessage("Maximum description length must be less that 150 symbols");
        
        RuleFor(x => x.City)
            .NotNull().WithMessage("City cannot be null")
            .NotEmpty().WithMessage("City cannot be empty")
            .MinimumLength(3).WithMessage("Minimum city length must be from 3 symbols")
            .MaximumLength(20).WithMessage("Maximum city length must be less that 20 symbols");
        
        RuleFor(x => x.StartEvent)
            .NotNull().WithMessage("Start date cannot be null")
            .Must(ValidateStartEvent).WithMessage("Start date must be greater than the current one");
    }

    private bool ValidateStartEvent(DateTime startEvent) => startEvent.ToUniversalTime() > DateTime.UtcNow;
}