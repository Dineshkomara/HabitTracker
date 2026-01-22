using AutoMapper;
using HabitTracker.Application.DTOs;
using HabitTracker.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Habit
        CreateMap<CreateHabitDto, Habit>();
        CreateMap<UpdateHabitDto, Habit>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore());
        CreateMap<Habit, HabitDto>();

        // HabitLog
        CreateMap<HabitLog, HabitLogDto>();
    }
}