using CommerceHub.Data.Context;
using CommerceHub.Data.Domain;
using CommerceHub.Data.Repositories.DapperRepository;
using CommerceHub.Data.Repositories.GenericRepository;
using CommerceHub.Data.Repositories.ReportRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CommerceHubDbContext dbContext;
        private readonly IDapperRepository dapperRepository;
        public IGenericRepository<User> UserRepository { get; }
        public IGenericRepository<Product> ProductRepository { get; }
        public IGenericRepository<Category> CategoryRepository { get; }
        public IGenericRepository<Coupon> CouponRepository { get; }
        public IGenericRepository<Order> OrderRepository { get; }
        public IReportRepository ReportRepository { get; }
        public UnitOfWork(CommerceHubDbContext dbContext,IDapperRepository dapperRepository)
        {
            this.dbContext = dbContext;
            this.dapperRepository = dapperRepository;
            UserRepository = new GenericRepository<User>(this.dbContext);
            ProductRepository = new GenericRepository<Product>(this.dbContext);
            CategoryRepository = new GenericRepository<Category>(this.dbContext);
            CouponRepository = new GenericRepository<Coupon>(this.dbContext);
            OrderRepository = new GenericRepository<Order>(this.dbContext);
            ReportRepository = new ReportRepository(this.dapperRepository);
        }
        public void Dispose()
        {
        }
        public async Task Complete()
        {
            await dbContext.SaveChangesAsync();
        }
        public async Task CompleteWithTransaction()
        {
            using (var dbTransaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await dbContext.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await dbTransaction.RollbackAsync();
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }
    }
}
