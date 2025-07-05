using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Repositories
{
    public class FoeVisitContext : IdentityDbContext<User>
    {

        public DbSet<Floor> Floors { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Department> Departments { get; set; }
		public DbSet<VisitorBlackList> VisitorBlackList { get; set; }
		//public DbSet<VisitorInVisit>  VisitorInVisits { get; set; }
		public FoeVisitContext(DbContextOptions<FoeVisitContext> options) : base(options)
        { }

         

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is FullAduitbaseModel referece)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            referece.CreatedDate = DateTime.Now;
                            break;
                        case EntityState.Deleted:
                        case EntityState.Modified:
                            referece.LastModifiedDate = DateTime.Now;
                            break;
                        default:
                            break;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Visits)
                .WithOne(v => v.CreatedUser)
                .HasForeignKey(v => v.CreatedUserId);
            modelBuilder.Entity<Visit>()
                .HasOne(v => v.CreatedUser)
                .WithMany(u => u.Visits);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithOne(Departmrnt => Departmrnt.UserAccount)
                .HasForeignKey<User>(u => u.DepartmentId);
            modelBuilder.Entity<User>()
                .HasKey(u => u.DepartmentId);
          //modelBuilder.Entity<VisitorInVisit>()
          //      .HasKey(v => new { v.VisitorId, v.VisitId });
			
			// Apply configurations from the assembly



			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Apply global query filter to all entities implementing ISoftDeletedModel
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletedModel).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(ISoftDeletedModel.IsDeleted));
                    var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }


        }

    }
}
