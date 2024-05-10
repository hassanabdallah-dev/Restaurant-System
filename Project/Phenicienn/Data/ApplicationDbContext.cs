using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Models;
using Phenicienn.Models.AdminUser;

namespace Phenicienn.Data
{
    public class ApplicationDbContext : IdentityDbContext<AdminUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrdersItems> OrdersItems { get; set; }
        public DbSet<CategoryFilter> CategoryFilters { get; set; }
        public DbSet<AdminUser> AdminUser { get; set; }
        public DbSet<Allergant> Allergants { get; set; }
        public DbSet<table> tables { get; set; }
        public DbSet<Bills> Bills { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CategoryFilter>()
                .HasOne(a => a.Category)
                .WithOne()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Restaurant>()
                .HasMany(i => i.Menus)
                .WithOne(m => m.Restaurant)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Restaurant>()
                .HasMany(i => i.tables)
                .WithOne(m => m.Restaurant)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasMany(m => m.Allergants)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Item>()
                .HasMany(m => m.Ingredients)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<table>()
                .HasIndex(m => m.TableNo)
                .IsUnique();
            modelBuilder.Entity<Order>()
                .HasMany(m => m.OrdersItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            /*modelBuilder.Entity<Menu>()
                .HasMany(i => i.Category)
                .WithOne(m => m.Menu);
              */  

            /*modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Menu)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}