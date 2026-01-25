using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TimeScale.DAL.Entities;

namespace TimeScale.DAL.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ValueEntity> Values { get; set; }
        public DbSet<ResultEntity> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
