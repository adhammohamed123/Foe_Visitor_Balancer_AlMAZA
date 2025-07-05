using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Extensions.Configurations.SeedData
{
    public class UserSeedDataConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                Id= "11111111-1111-1111-1111-111111111111", // Guid as string
                DepartmentId = "11111111-1111-1111-1111-111111111111",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "adhammo909@gmail.com",
                //DepartmentId = 1
            };
            user.PasswordHash = hasher.HashPassword(user, "SuperAdmin123");

           

            builder.HasData(user);
            builder.Property(u => u.Id).HasMaxLength(64);
        }
    }


    public class DeptSeedDataConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(
                new Department
                {
                    Id = "11111111-1111-1111-1111-111111111111",
                    Name = "nozom"
                   
                }
            );
        }
    }

}
