using FluentValidation;
using HabitTracker.Application.Interfaces;
using HabitTracker.Application.Mappings;
using HabitTracker.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HabitTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssemblyContaining<Validators.CreateHabitValidator>();
        services.AddScoped<IHabitService, HabitService>();

        return services;
    }
}