using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeScale.DAL.EF;
using TimeScale.DAL.Interfaces;
using TimeScale.DAL.Repositories;

namespace TimeScale.DAL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAssessmentRepository, AssessmentRepository>();

            return services;
        }
    }
}
