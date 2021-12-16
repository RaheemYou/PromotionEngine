using FluentValidation;
using PromotionEngine.Models.BusinessModels;
using System.Linq;

namespace PromotionEngine.Validators
{
    public class CartValidator : AbstractValidator<CartModel>
    {
        public CartValidator()
        {
            RuleFor(x => x.CartItems)
                .NotNull()
                .NotEmpty()
                .WithMessage("Cart Items Must be specified");

            RuleFor(x => x)
                .NotNull()
                .WithMessage("Cart must not be null");

            RuleFor(x => x)
                .Must(x => x.CartItems.Select(y => y.SKU).Distinct().Count() == x.CartItems.Count)
                .NotEmpty()
                .WithMessage("Cart Items must be grouped");
        }
    }
}
