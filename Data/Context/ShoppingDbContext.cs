using Entity.Abstract;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shopping.Data.EntityMap;

namespace Shopping.Data.Context
{
    public class ShoppingDbContext : DbContext
    {

        public ShoppingDbContext()
        {
        }
        public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ProductList> ProductLists { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<UserProduct> UserProducts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=ShoppingDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new ProductListMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new UserProductMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
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
                if (entity is BaseModel)
                {
                    if (entity is BaseModel track)
                    {
                        track.CreatedDate = DateTime.UtcNow;
                    }
                }
            }

            var modified = ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Modified)
                        .Select(t => t.Entity)
                        .ToArray();

            foreach (var entity in modified)
            {
                if (entity is BaseModel)
                {
                    if (entity is BaseModel track)
                    {
                        track.ModifiedDate = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
