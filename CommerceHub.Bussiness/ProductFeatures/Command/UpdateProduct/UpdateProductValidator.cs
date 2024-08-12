using CommerceHub.Bussiness.CategoryFeatures.Validation;
using CommerceHub.Bussiness.ProductFeatures.Command.CreateProduct;
using CommerceHub.Bussiness.ProductFeatures.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ProductFeatures.Command.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Product Id is required.");
            RuleFor(command => command.Request).SetValidator(new ProductRequestBaseValidator());
        }
    }
}
