using CommerceHub.Bussiness.CouponFeatures.Command.UpdateCoupon;
using CommerceHub.Bussiness.CouponFeatures.Validation;
using CommerceHub.Bussiness.UserFeatures.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.UserFeatures.Command.UpdateUser
{
    internal class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("User Id is required.");
            RuleFor(command => command.Request).SetValidator(new UserRequestBaseValidation());
        }
    }
}
