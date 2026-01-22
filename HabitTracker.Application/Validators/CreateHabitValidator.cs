using FluentValidation;
using HabitTracker.Application.DTOs;

namespace HabitTracker.Application.Validators;

public class CreateHabitValidator : AbstractValidator<CreateHabitDto>
{
    public CreateHabitValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Habit name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Frequency)
            .NotEmpty().WithMessage("Frequency must be selected.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .Must(date => date.Date >= DateTime.UtcNow.Date)
            .WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("End date must be after start date.");
    }
}