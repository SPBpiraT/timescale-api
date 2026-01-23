using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeScale.DAL.Entities;

namespace TimeScale.DAL.EF.EntityTypeConfiguration
{
    public class ResultConfiguration : IEntityTypeConfiguration<ResultEntity>
    {
        public void Configure(EntityTypeBuilder<ResultEntity> builder)
        {
            builder.HasKey(result => result.Id);
        }
    }
}
