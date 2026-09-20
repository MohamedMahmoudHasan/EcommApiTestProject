using Lab11.Data.Models;
using Lab11.Data.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lab11.Data.Context
{
    public class EComAppDbContext: DbContext
    {
        public EComAppDbContext(){}
        public EComAppDbContext(DbContextOptions<EComAppDbContext> options):base(options) {}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=. ;DataBase=EComAppDbLab2;Trusted_Connection = true;TrustServerCertificate = true");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Product>(new ProductConfigurations());
            modelBuilder.ApplyConfiguration<Category>(new CategoryConfigurations());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
