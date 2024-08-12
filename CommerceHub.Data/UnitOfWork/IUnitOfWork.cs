using CommerceHub.Data.Domain;
using CommerceHub.Data.Repositories.GenericRepository;
using CommerceHub.Data.Repositories.ReportRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task Complete();
        Task CompleteWithTransaction();
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Coupon> CouponRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IReportRepository ReportRepository { get; }
    }
}
