using BBMDemo.Data.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBMDemo.Data.Data
{
    public class BBMContext: IdentityDbContext<User>
    {
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductInventory> ProductInventory { get; set; }
        public virtual DbSet<Style> Style { get; set; }

        public BBMContext(DbContextOptions<BBMContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category", "Production");
            });           

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "Production");
            });           

            modelBuilder.Entity<Style>(entity =>
            {
                entity.ToTable("Style", "Production");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "Production");
            });

            modelBuilder.Entity<ProductInventory>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.StyleId, e.LocationId });

                entity.ToTable("ProductInventory", "Production");

            });
        }
    }
}
