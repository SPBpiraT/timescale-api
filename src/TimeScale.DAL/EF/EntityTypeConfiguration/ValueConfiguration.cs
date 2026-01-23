using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeScale.DAL.Entities;

namespace TimeScale.DAL.EF.EntityTypeConfiguration
{
    public class ValueConfiguration : IEntityTypeConfiguration<ValueEntity>
    {
        public void Configure(EntityTypeBuilder<ValueEntity> builder)
        {
            builder.HasKey(value => value.Id);
            builder.HasIndex(value => value.Id).IsUnique();
        }
    }
}
