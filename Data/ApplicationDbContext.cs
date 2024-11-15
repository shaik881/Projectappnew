using DefaultIdentityColumnRename.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DefaultIdentityColumnRename.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Remove columns not present in ApplicationUser, or rename fields using Fluent API
            builder.Entity<User>().Ignore(u => u.PhoneNumber);
           // builder.Entity<User>().Property(u => u.UserName).HasColumnName("UserName");
           
            // Example of ignoring a property
            builder.Entity<UserProfile>(entity=>
            {
                entity.Property(e => e.Address)
                 .IsRequired(false);

               

                entity.Property(e => e.State)
                      .IsRequired(false);

                entity.Property(e => e.Gender)
                      .IsRequired(false);
                entity.Property(e=>e.Pic).IsRequired(false);
                  });
        }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
    }
}
