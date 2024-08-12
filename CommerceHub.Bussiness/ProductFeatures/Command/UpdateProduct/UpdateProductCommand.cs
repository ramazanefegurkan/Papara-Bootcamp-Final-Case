
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ProductFeatures.Command.UpdateProduct
{
    public record UpdateProductCommand(int Id, BasicProductRequest Request) : IRequest<ApiResponse<bool>>;
}
