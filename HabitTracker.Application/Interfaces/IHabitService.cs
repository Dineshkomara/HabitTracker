using HabitTracker.Application.DTOs;

namespace HabitTracker.Application.Interfaces;

public interface IHabitService
{
    // Habit CRUD
    Task<List<HabitDto>> GetAllAsync();
    Task<HabitDto?> GetByIdAsync(Guid id);
    Task<HabitDto> CreateAsync(CreateHabitDto dto);
    Task<HabitDto?> UpdateAsync(Guid id, UpdateHabitDto dto);
    Task<bool> DeleteAsync(Guid id);

    // Habit Logs
    Task<HabitLogDto?> LogHabitAsync(Guid habitId, CreateHabitLogDto dto);
    Task<List<HabitLogDto>> GetLogsAsync(Guid habitId);

    // Stats
    Task<HabitStatsDto?> GetStatsAsync(Guid habitId);
}