using Application.Common.Behaviors;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Hosting;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddAplicationServices(this IHostApplicationBuilder builder) 
    {
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Services.AddMediatR(configuration => {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
            configuration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
    }
}