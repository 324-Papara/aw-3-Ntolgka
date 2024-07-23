using FluentValidation;
using Para.Schema;

namespace Para.Bussiness.Validation;

public class CustomerValidator : AbstractValidator<CustomerRequest>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.FirstName).NotNull()
            .WithMessage("First name is required.")
            .MaximumLength(50)
            .WithMessage("First name cannot be greater than 50 characters.");
        RuleFor(customer => customer.LastName).NotNull()
            .WithMessage("Last name is required.")
            .MaximumLength(50)
            .WithMessage("Last name cannot be greater than 50 characters.");
        RuleFor(customer => customer.IdentityNumber).NotNull()
            .WithMessage("Identity number is required.")
            .MaximumLength(11)
            .WithMessage("Identity number cannot be greater than 11.");
        RuleFor(customer => customer.Email).NotNull()
            .WithMessage("Email is required")
            .MaximumLength(100)
            .WithMessage("Email cannot be greater than 100 characters.");
        RuleFor(customer => customer.CustomerNumber).NotNull()
            .WithMessage("Customer number is required.");
        RuleFor(customer => customer.DateOfBirth).NotNull()
            .WithMessage("Date of birth is required.");
    }
}