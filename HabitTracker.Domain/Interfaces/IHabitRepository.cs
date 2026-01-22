// Domain/Interfaces/IHabitRepository.cs
public interface IHabitRepository
{
    Task<IEnumerable<Habit>> GetAllAsync();
    Task<Habit?> GetByIdAsync(Guid id);
    Task AddAsync(Habit habit);
    void Update(Habit habit);
    void Delete(Habit habit);
    Task SaveAsync();
}

// Domain/Interfaces/IHabitLogRepository.cs
public interface IHabitLogRepository
{
    Task<IEnumerable<HabitLog>> GetByHabitIdAsync(Guid habitId);
    Task AddAsync(HabitLog log);
    Task SaveAsync();
}