using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class RoleAssignmentConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "7b72a55e-0189-4665-87ab-b8c4a44e00f0",
                    RoleId = "cfa9978f-2afd-4786-9cf9-97b4493f4d34"
                }
            );
        }
    }
}
