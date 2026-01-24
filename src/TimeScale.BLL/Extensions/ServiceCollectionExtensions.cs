using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TimeScale.BLL.Interfaces;
using TimeScale.BLL.Models.Value;
using TimeScale.BLL.Services;
using TimeScale.BLL.Validators;

namespace TimeScale.BLL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFetchDataService, FetchDataService>();
            services.AddScoped<IUploadDataService, UploadDataService>();

            return services;
        }

        public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<ValueDto>, CSVDataValidator>();

            return services;
        }
    }
}
