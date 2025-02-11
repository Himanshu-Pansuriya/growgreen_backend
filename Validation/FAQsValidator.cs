using FluentValidation;
using growgreen_backend.Models;

namespace growgreen_backend.Validation
{
    public class FAQModelValidator : AbstractValidator<FAQModel>
    {
        public FAQModelValidator()
        {
            RuleFor(faq => faq.Question).NotEmpty()
                .WithMessage("Question is required.");
            
            RuleFor(faq => faq.Answer).NotEmpty()
                .WithMessage("Answer is required.");
        }
    }
}
