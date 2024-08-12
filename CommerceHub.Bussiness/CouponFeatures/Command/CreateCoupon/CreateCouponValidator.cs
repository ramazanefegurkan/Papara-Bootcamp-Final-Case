using CommerceHub.Bussiness.CategoryFeatures.Command.CreateCategory;
using CommerceHub.Bussiness.CategoryFeatures.Validation;
using CommerceHub.Bussiness.CouponFeatures.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CouponFeatures.Command.CreateCoupon
{
    public class CreateCouponValidator : AbstractValidator<CreateCouponCommand>
    {
        public CreateCouponValidator()
        {
            RuleFor(command => command.Request).SetValidator(new CouponRequestBaseValidator());
        }
    }
}
