using CommerceHub.Base.Entity;
using CommerceHub.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Domain
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public UserRole Role { get; set; } = UserRole.Normal;
        public decimal WalletBalance { get; set; } = 0;

        public ICollection<Coupon> CreatedCoupons { get; set; } = new List<Coupon>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
