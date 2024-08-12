using CommerceHub.Bussiness.CategoryFeatures.Command.UpdateCategory;
using CommerceHub.Bussiness.CategoryFeatures.Validation;
using CommerceHub.Bussiness.CouponFeatures.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CouponFeatures.Command.UpdateCoupon
{
    internal class UpdateCouponValidator : AbstractValidator<UpdateCouponCommand>
    {
        public UpdateCouponValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Coupon Id is required.");
            RuleFor(command => command.Request).SetValidator(new CouponRequestBaseValidator());
        }
    }
}
