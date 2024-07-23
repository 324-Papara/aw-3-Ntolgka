using FluentValidation;
using Para.Schema;

namespace Para.Bussiness.Validation;

public class CustomerPhoneValidator : AbstractValidator<CustomerPhoneRequest>
{
    public CustomerPhoneValidator()
    {
        RuleFor(customerPhone => customerPhone.CountyCode).NotNull()
            .WithMessage("Country code is required.")
            .MaximumLength(3)
            .WithMessage("Country code cannot be greater than 3 characters.");
        RuleFor(customerPhone => customerPhone.Phone).NotNull()
            .WithMessage("Phone number is required.")
            .MaximumLength(10)
            .WithMessage("Phone number cannot be greater than 10 characters.");
        RuleFor(customerPhone => customerPhone.IsDefault).NotNull()
            .WithMessage("Customer phone number is required.");
    }
}