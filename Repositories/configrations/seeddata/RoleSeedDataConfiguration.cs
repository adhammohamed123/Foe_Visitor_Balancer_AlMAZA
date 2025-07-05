using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions.Configurations.SeedData
{
    public class RoleSeedDataConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                  new IdentityRole
                {
                    Id = "11111111-1111-1111-1111-111111111111",
                    Name = "nozom",
                    NormalizedName = "NOZOM"
                },
                  new IdentityRole
                {
                    Id = "C0969547-A084-4839-836C-F41F4CF5D211",
                    Name = "dept",
                    NormalizedName = "DEPT"
                },
                  new IdentityRole
                {
                    Id = "C0969547-A084-4839-836C-F41F4CF5D212",
                    Name = "police",
                    NormalizedName = "POLICE"
                },
                  new IdentityRole
                  {
                      Id = "C0969547-A084-4839-836C-F41F4CF5D222",
                      Name = "gate",
                      NormalizedName = "GATE"
                  },
                  new IdentityRole
                  {
                      Id = "C0969547-A084-4839-836C-F41F4CF5D121",
                      Name = "secertary",
                      NormalizedName = "SECERTARY"
                  }
                );
        }
    }
}
