using CommerceHub.Bussiness.Exceptions;
using CommerceHub.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class FetchProductsHandler : IOrderHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public int Order => 1;

        public FetchProductsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(OrderContext context)
        {
            var productIds = context.OrderRequest.Items.Select(i => i.ProductId).ToList();
            var products = await _unitOfWork.ProductRepository
                .GetAll(p => productIds.Contains(p.Id));

            if (products.Count != productIds.Count)
            {
                var notFoundProductIds = productIds.Except(products.Select(p => p.Id)).ToList();
                throw new NotFoundException($"Products not found: {string.Join(", ", notFoundProductIds)}");
            }

            context.Products = products;

            await Task.CompletedTask;
        }
    }
}
