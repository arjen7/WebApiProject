using Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data.EntityMap
{
    public class ProductListMap : BaseModelMap<ProductList>, IEntityTypeConfiguration<ProductList>
    {
        public new void Configure(EntityTypeBuilder<ProductList> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Name).IsRequired();
            builder.HasMany(t => t.UserProducts).WithOne(t => t.ProductList).HasForeignKey(t => t.ProductId);
            builder.HasIndex(t=>t.UserId);
            builder.HasOne(t=>t.User).WithMany(t=>t.ProductLists).HasForeignKey(t=>t.UserId).IsRequired();
        }
    }
}
