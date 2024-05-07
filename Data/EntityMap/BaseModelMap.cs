using Entity.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shopping.Data.EntityMap
{
    public class BaseModelMap<T> where T : BaseModel
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(t => t.ModifiedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
    
}
