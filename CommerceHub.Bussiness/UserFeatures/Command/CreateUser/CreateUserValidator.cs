using CommerceHub.Bussiness.CouponFeatures.Validation;
using CommerceHub.Bussiness.UserFeatures.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.UserFeatures.Command.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(command => command.Request).SetValidator(new UserRequestBaseValidation());
        }
    }
}
