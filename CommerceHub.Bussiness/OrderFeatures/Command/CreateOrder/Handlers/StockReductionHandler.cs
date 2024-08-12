using CommerceHub.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers
{
    public class StockReductionHandler : IOrderHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        public int Order => 8;

        public StockReductionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task Handle(OrderContext context)
        {
            foreach (var item in context.Order.OrderDetails)
            {
                var product = context.Products.First(x => x.Id == item.ProductId);

                product.StockQuantity -= item.Quantity;
                _unitOfWork.ProductRepository.Update(product);
            }

            await Task.CompletedTask;
        }
    }
}
