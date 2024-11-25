using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                    UserName = "NguoiDev",
                    NormalizedUserName = "NGUOIDEV",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<User>().HashPassword(null, "123123a123123"),
                    SecurityStamp = string.Empty,
                }
            );
        }
    }
}
