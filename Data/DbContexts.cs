using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AddressBook.Models;

namespace AddressBook.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }

    public class AddressBookDbContext : DbContext
    {
        public DbSet<ContactPerson> ContatPersons { get; set; }
        public DbSet<ContactAddress> ContactAddresses { get; set;}

        public AddressBookDbContext(DbContextOptions<AddressBookDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactPerson>().ToTable("ContactPerson");
            modelBuilder.Entity<ContactAddress>().ToTable("ContactAddress");
        }

    }
}
