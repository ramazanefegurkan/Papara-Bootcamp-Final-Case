using CommerceHub.Schema;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ProductFeatures.Validation
{
    internal class ProductRequestBaseValidator : AbstractValidator<BasicProductRequest>
    {
        public ProductRequestBaseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.RewardPointsPercentage)
                .InclusiveBetween(0, 100).WithMessage("RewardPointsPercentage must be between 0 and 100.");

            RuleFor(x => x.MaxRewardPoints)
                .GreaterThanOrEqualTo(0).WithMessage("MaxRewardPoints must be greater than or equal to zero.");

            RuleFor(x => x.CategoryIds)
                .NotNull().WithMessage("CategoryIds is required.")
                .Must(x => x.Count > 0).WithMessage("At least one CategoryId is required.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value.");
        }
    }
}
