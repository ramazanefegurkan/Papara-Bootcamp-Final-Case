using CommerceHub.Schema;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ProductFeatures.Command.UpdateProductStocck
{
    public record UpdateProductStockCommand(int Id, UpdateProductStockRequest Request) : IRequest<ApiResponse<bool>>;
}
