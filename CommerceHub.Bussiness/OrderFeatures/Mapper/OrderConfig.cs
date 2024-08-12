using AutoMapper;
using CommerceHub.Data.Domain;
using CommerceHub.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.OrderFeatures.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderResponse>();
            CreateMap<OrderDetail, OrderDetailResponse>();
        }
    }
}
