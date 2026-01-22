using AutoMapper;
using HabitTracker.Application.DTOs;
using HabitTracker.Application.Interfaces;
using HabitTracker.Domain.Entities;
using HabitTracker.Domain.Interfaces;

namespace HabitTracker.Application.Services;

public class HabitService : IHabitService
{
    private readonly IHabitRepository _habitRepo;
    private readonly IHabitLogRepository _logRepo;
    private readonly IMapper _mapper;

    public HabitService(IHabitRepository habitRepo, IHabitLogRepository logRepo, IMapper mapper)
    {
        _habitRepo = habitRepo;
        _logRepo = logRepo;
        _mapper = mapper;
    }

    // ===== HABIT CRUD =====

    public async Task<List<HabitDto>> GetAllAsync()
    {
        var habits = await _habitRepo.GetAllAsync();
        return _mapper.Map<List<HabitDto>>(habits);
    }

    public async Task<HabitDto?> GetByIdAsync(Guid id)
    {
        var habit = await _habitRepo.GetByIdAsync(id);
        return habit == null ? null : _mapper.Map<HabitDto>(habit);
    }

    public async Task<HabitDto> CreateAsync(CreateHabitDto dto)
    {
        var habit = _mapper.Map<Habit>(dto);
        habit.Id = Guid.NewGuid();
        habit.Status = "YetToStart";
        habit.CreatedAt = DateTime.UtcNow;

        await _habitRepo.AddAsync(habit);
        await _habitRepo.SaveAsync();

        return _mapper.Map<HabitDto>(habit);
    }

    public async Task<HabitDto?> UpdateAsync(Guid id, UpdateHabitDto dto)
    {
        var habit = await _habitRepo.GetByIdAsync(id);
        if (habit == null) return null;

        _mapper.Map(dto, habit);
        _habitRepo.Update(habit);
        await _habitRepo.SaveAsync();

        return _mapper.Map<HabitDto>(habit);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var habit = await _habitRepo.GetByIdAsync(id);
        if (habit == null) return false;

        _habitRepo.Delete(habit);
        await _habitRepo.SaveAsync();
        return true;
    }

    // ===== HABIT LOGS =====

    public async Task<HabitLogDto?> LogHabitAsync(Guid habitId, CreateHabitLogDto dto)
    {
        var habit = await _habitRepo.GetByIdAsync(habitId);
        if (habit == null) return null;

        var log = new HabitLog
        {
            Id = Guid.NewGuid(),
            HabitId = habitId,
            LogDate = dto.LogDate.Date,
            IsCompleted = true,
            Notes = dto.Notes
        };

        await _logRepo.AddAsync(log);
        await _logRepo.SaveAsync();

        // Update habit status to Active
        if (habit.Status == "YetToStart")
        {
            habit.Status = "Active";
            _habitRepo.Update(habit);
            await _habitRepo.SaveAsync();
        }

        return _mapper.Map<HabitLogDto>(log);
    }

    public async Task<List<HabitLogDto>> GetLogsAsync(Guid habitId)
    {
        var logs = await _logRepo.GetByHabitIdAsync(habitId);
        return _mapper.Map<List<HabitLogDto>>(logs);
    }

    // ===== STATS =====

    public async Task<HabitStatsDto?> GetStatsAsync(Guid habitId)
    {
        var habit = await _habitRepo.GetByIdAsync(habitId);
        if (habit == null) return null;

        var logs = await _logRepo.GetByHabitIdAsync(habitId);
        var completedLogs = logs.Where(l => l.IsCompleted).OrderByDescending(l => l.LogDate).ToList();

        return new HabitStatsDto
        {
            HabitId = habitId,
            HabitName = habit.Name,
            CurrentStreak = CalculateStreak(completedLogs),
            TotalCompletions = completedLogs.Count,
            CompletionRate = CalculateCompletionRate(habit.StartDate, completedLogs.Count)
        };
    }

    private int CalculateStreak(List<HabitLog> logs)
    {
        if (!logs.Any()) return 0;

        int streak = 0;
        var today = DateTime.UtcNow.Date;

        foreach (var log in logs)
        {
            var expectedDate = today.AddDays(-streak);
            if (log.LogDate.Date == expectedDate)
                streak++;
            else
                break;
        }

        return streak;
    }

    private double CalculateCompletionRate(DateTime startDate, int completions)
    {
        var totalDays = (DateTime.UtcNow.Date - startDate.Date).Days + 1;
        if (totalDays <= 0) return 0;
        return Math.Round((double)completions / totalDays * 100, 2);
    }
}