namespace HabitTracker.Application.DTOs;

public class CreateHabitLogDto
{
    public DateTime LogDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}