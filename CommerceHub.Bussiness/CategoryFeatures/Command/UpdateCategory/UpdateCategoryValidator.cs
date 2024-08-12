using CommerceHub.Bussiness.CategoryFeatures.Command.CreateCategory;
using CommerceHub.Bussiness.CategoryFeatures.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CategoryFeatures.Command.UpdateCategory
{
    internal class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Category Id is required.");
            RuleFor(command => command.Request).SetValidator(new CategoryRequestBaseValidator());
        }
    }
}
