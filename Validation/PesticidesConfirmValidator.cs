using FluentValidation;
using growgreen_backend.Models;

namespace growgreen_backend.Validation
{
    public class PesticidesConfirmModelValidator : AbstractValidator<PesticidesConfirmModel>
    {
        public PesticidesConfirmModelValidator()
        {
            RuleFor(pesticide => pesticide.PesticidesTransactionID)
                .GreaterThan(0).WithMessage("PesticidesTransactionID must be greater than zero.");
            
            RuleFor(pesticide => pesticide.SalesmanID)
                .GreaterThan(0).WithMessage("SalesmanID must be greater than zero.");
            
            RuleFor(pesticide => pesticide.Status)
                 .NotEmpty().WithMessage("Status is required.");
        }
    }
}
