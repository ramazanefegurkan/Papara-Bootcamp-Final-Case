
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.ProductFeatures.Query.GetProductById
{
    public record GetProductByIdQuery(int Id) : IRequest<ApiResponse<ProductDetailResponse>>;
}
