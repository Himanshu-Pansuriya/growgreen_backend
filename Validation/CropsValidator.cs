using FluentValidation;
using growgreen_backend.Models;

namespace growgreen_backend.Validation
{
    public class CropsModelValidator : AbstractValidator<CropsModel>
    {
        public CropsModelValidator()
        {
            RuleFor(crop => crop.FarmerID)
                .GreaterThan(0).WithMessage("FarmerID must be greater than zero.");

            RuleFor(crop => crop.CropName)
                .NotEmpty().WithMessage("Crop name is required.");
            
            RuleFor(crop => crop.CropType)
                .NotEmpty().WithMessage("Crop type is required.");
            
            RuleFor(crop => crop.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            
            RuleFor(crop => crop.PricePer20KG)
                .GreaterThan(0).WithMessage("Price per 20KG must be greater than zero.");
            
            RuleFor(crop => crop.ContactNo)
                .Matches("^[0-9]{10}$").WithMessage("Contact number must be 10 digits.");
            
            RuleFor(crop => crop.Address)
                .NotEmpty().WithMessage("Address is required.");
            
            RuleFor(crop => crop.status)
                .NotEmpty().WithMessage("Status is required.");
        }
    }
}
