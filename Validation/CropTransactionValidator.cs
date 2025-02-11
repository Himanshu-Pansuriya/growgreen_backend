using FluentValidation;
using growgreen_backend.Models;

namespace growgreen_backend.Validation
{
    public class CropsTransactionModelValidator : AbstractValidator<CropsTransactionModel>
    {
        public CropsTransactionModelValidator()
        {
            RuleFor(transaction => transaction.BuyerID)
                .GreaterThan(0).WithMessage("BuyerID must be greater than zero.");
            
            RuleFor(transaction => transaction.CropID)
                .GreaterThan(0).WithMessage("CropID must be greater than zero.");
            
            RuleFor(transaction => transaction.QuantityPurchased)
                .GreaterThan(0).WithMessage("Quantity purchased must be greater than zero.");
            
            RuleFor(transaction => transaction.TotalPrice)
                .GreaterThan(0).WithMessage("Total price must be greater than zero.");
            
            RuleFor(transaction => transaction.PurchaseDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Purchase date cannot be in the future.");
        }
    }
}
