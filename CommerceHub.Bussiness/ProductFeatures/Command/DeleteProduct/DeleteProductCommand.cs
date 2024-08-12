
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ProductFeatures.Command.DeleteProduct
{
    public record DeleteProductCommand(int Id) : IRequest<ApiResponse<bool>>;
}
