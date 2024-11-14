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
            
           
            // Example of ignoring a property
        }
    }
}
