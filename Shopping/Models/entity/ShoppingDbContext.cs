
using Microsoft.EntityFrameworkCore;
using Shopping.Interfaces;
using Shopping.Services;

namespace Shopping.Models.entity
{
    public class ShoppingDbContext : DbContext
    {
        public int UserId { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductList> ProductLists { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.;Database=ShoppingDb;Trusted_Connection=True;TrustServerCertificate=True;",
                    options => options.EnableRetryOnFailure());
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var saltvalue = PasswordService.GenerateSalt();
            var password = PasswordService.HashPassword("Abcd123!", saltvalue);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@gmail.com",
                    PasswordHash = password,
                    SaltValue = saltvalue,
                    Name = "admin",
                    LastName = "admin",
                    Role = "Admin"
                });
            modelBuilder.Entity<UserProduct>()
            .HasOne(up => up.Product)
            .WithMany(p => p.UserProducts)
            .HasForeignKey(up => up.ProductId);

            modelBuilder.Entity<UserProduct>()
                .HasOne(up => up.ProductList)
                .WithMany(pl => pl.UserProducts)
                .HasForeignKey(up => up.ProductListId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<ProductList>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.ProductLists)
                .HasForeignKey(pl => pl.UserId);

            modelBuilder.Entity<User>().HasIndex(p=>p.Email).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();




        }
        public override int SaveChanges()
        {
            AutoLog();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AutoLog();
            return base.SaveChangesAsync(cancellationToken);
        }
        protected void AutoLog()
        {
            ChangeTracker.DetectChanges();
            var added = ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Added)
                        .Select(t => t.Entity)
                        .ToArray();

            foreach (var entity in added)
            {
                if (entity is IBaseModel)
                {
                    if (entity is IBaseModel track)
                    {
                        track.CreatedDate = DateTime.Now;
                        track.CreatedBy = UserId;
                    }
                }
            }

            var modified = ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Modified)
                        .Select(t => t.Entity)
                        .ToArray();

            foreach (var entity in modified)
            {
                if (entity is IBaseModel)
                {
                    if (entity is IBaseModel track)
                    {
                        track.ModifiedDate = DateTime.Now;
                        track.ModifiedBy = UserId;
                    }
                }
            }
        }
    }
}
