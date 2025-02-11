using FluentValidation;
using growgreen_backend.Models;

namespace growgreen_backend.Validation
{
    public class UserValidator:AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(u => u.UserName)
               .NotEmpty().WithMessage("User Name can't be empty")
               .NotNull().WithMessage("User Name can't be null");


            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email can't be empty")
                .EmailAddress().WithMessage("Invalid Email format");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password can't be empty")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long");

            RuleFor(u => u.Role)
                .NotEmpty().WithMessage("Role can't be empty")
                .Must(mode => new[] { "Admin", "Salesmen", "Farmer" }.Contains(mode))
                .WithMessage("Role must be one of the following: Admin, Salesmen, Farmer ");

        }
    }
}
