using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Behaviors;

namespace Template.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMediatRDependency(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddHttpClient("ekyc", c => {  });
            
            return services;
        }
    }
}