using FluentValidation;
using growgreen_backend.Models;

namespace growgreen_backend.Validation
{
    public class BlogModelValidator : AbstractValidator<BlogModel>
    {
        public BlogModelValidator()
        {
            RuleFor(blog => blog.AdminID)
                .GreaterThan(0).WithMessage("AdminID must be greater than zero.");

            RuleFor(blog => blog.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(blog => blog.Detail)
                .NotEmpty().WithMessage("Detail is required.");

            RuleFor(blog => blog.PublishedDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Published date cannot be in the future.");
        }
    }
}
