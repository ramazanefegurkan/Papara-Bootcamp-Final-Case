using CommerceHub.Schema;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.CategoryFeatures.Validation
{
    internal class CategoryRequestBaseValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestBaseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Category URL is required.")
                .MaximumLength(200).WithMessage("Category URL must not exceed 200 characters.")
                .Matches(@"^[a-zA-Z0-9\-]+$").WithMessage("Category URL can only contain alphanumeric characters and hyphens.");

            RuleFor(x => x.Tags)
                .MaximumLength(500).WithMessage("Category tags must not exceed 500 characters.");
        }
    }
}
