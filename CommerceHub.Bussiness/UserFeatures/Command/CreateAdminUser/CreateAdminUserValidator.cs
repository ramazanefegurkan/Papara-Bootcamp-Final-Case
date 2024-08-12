using CommerceHub.Bussiness.UserFeatures.Command.CreateUser;
using CommerceHub.Bussiness.UserFeatures.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.UserFeatures.Command.CreateAdminUser
{
    public class CreateUserValidator : AbstractValidator<CreateAdminUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(command => command.Request).SetValidator(new UserRequestBaseValidation());
        }
    }
}
