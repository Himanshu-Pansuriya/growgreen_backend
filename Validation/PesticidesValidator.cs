using FluentValidation;
using growgreen_backend.Models;

namespace growgreen_backend.Validation
{
    public class PesticidesValidator : AbstractValidator<PesticidesModel>
    {
        public PesticidesValidator()
        {
            
            RuleFor(pesticide => pesticide.PesticidesName)
                .NotEmpty().WithMessage("Pesticide name is required.");
            
            RuleFor(pesticide => pesticide.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
            
            RuleFor(pesticide => pesticide.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");
            
            RuleFor(pesticide => pesticide.ExpiryDate)
                .GreaterThan(DateTime.Now).WithMessage("Expiry date must be in the future.");
        
        }
    }
}
