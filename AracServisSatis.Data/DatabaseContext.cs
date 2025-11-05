using AracServisSatis.Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

namespace AracServisSatis.Data
{
    public class DatabaseContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
        {
        }
        public DbSet<Arac> Araclar { get; set; }
        public DbSet<Marka> Markalar { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Satis> Satislar { get; set; }
        public DbSet<Servis> Servisler { get; set; }
        public DbSet<Slider> Sliders { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=.\SQLEXPRESS; database=AracServisSatisNetCore; integrated security=True;  TrustServerCertificate=true;");
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore
            (RelationalEventId.PendingModelChangesWarning));
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API
            modelBuilder.Entity<Marka>().Property(m => m.Adi).IsRequired().HasColumnType("varchar(50)");
           
       
            base.OnModelCreating(modelBuilder);
        }
    }
}