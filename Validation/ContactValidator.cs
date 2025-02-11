using FluentValidation;
using growgreen_backend.Models;

namespace growgreen_backend.Validation
{
    public class ContactModelValidator : AbstractValidator<ContactModel>
    {
        public ContactModelValidator()
        {

            RuleFor(contact => contact.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(contact => contact.MobileNo)
                .Matches("^[0-9]{10}$").WithMessage("Mobile number must be exactly 10 digits.");

            RuleFor(contact => contact.Email)
                .EmailAddress().When(contact => !string.IsNullOrEmpty(contact.Email)).WithMessage("Invalid email format.");

            RuleFor(contact => contact.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
