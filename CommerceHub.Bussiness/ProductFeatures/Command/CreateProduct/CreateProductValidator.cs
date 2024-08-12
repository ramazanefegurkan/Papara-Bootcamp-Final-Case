using CommerceHub.Bussiness.CategoryFeatures.Validation;
using CommerceHub.Bussiness.ProductFeatures.Command.CreateProduct;
using CommerceHub.Bussiness.ProductFeatures.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ProductFeatures.Command.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(command => command.Request).SetValidator(new ProductRequestBaseValidator());

            RuleFor(x => x.Request.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("StockQuantity must be greater than or equal to zero.");
        }
    }
}
