using CommerceHub.Base.Helper.PasswordHasher;
using CommerceHub.Data.Context;
using CommerceHub.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Data.Seeder
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CommerceHubDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<CommerceHubDbContext>>()))
            {
                // Veritabanını oluştur
                context.Database.EnsureCreated();

                // Eğer admin kullanıcı yoksa ekle
                if (!context.Users.Any(u => u.UserName == "test" && u.Role == Enums.UserRole.Admin))
                {
                    var adminUser = new User
                    {
                        UserName = "test",
                        PasswordHash = PasswordHasher.HashPassword("test"), // Parolanın hashlenmesi
                        Role = Enums.UserRole.Admin,
                        DateOfBirth = DateTime.UtcNow,
                        FirstName = "Test",
                        Email = "test@test.com",
                        LastName = "Test",
                        PhoneNumber = "1234567890",
                        WalletBalance = 0
                    };
                    context.Users.Add(adminUser);
                    context.SaveChanges();
                }
            }
        }
    }
}
