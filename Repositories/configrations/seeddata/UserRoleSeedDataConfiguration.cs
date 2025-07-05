using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Extensions.Configurations.SeedData
{
    public class UserRoleSeedDataConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "11111111-1111-1111-1111-111111111111",
                    UserId = "11111111-1111-1111-1111-111111111111"
                }
                );
        }
    }
}
