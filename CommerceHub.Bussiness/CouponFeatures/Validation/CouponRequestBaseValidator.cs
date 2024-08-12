using CommerceHub.Bussiness.CategoryFeatures.Command.CreateCategory;
using CommerceHub.Schema;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CouponFeatures.Validation
{
    internal class CouponRequestBaseValidator : AbstractValidator<CouponRequest>
    {
        public CouponRequestBaseValidator()
        {
            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("Coupon code is required.")
                .MaximumLength(10).WithMessage("Coupon code cannot exceed 10 characters.")
                .Matches("^[A-Za-z0-9]+$").WithMessage("Coupon code must be alphanumeric.");

            RuleFor(c => c.Amount)
                .GreaterThan(0).WithMessage("Coupon amount must be greater than zero.")
                .WithMessage("Coupon amount is required.");

            RuleFor(c => c.ExpiryDate)
                .GreaterThan(DateTime.Now).WithMessage("Expiry date must be in the future.")
                .WithMessage("Expiry date is required.");
        }
    }
}
