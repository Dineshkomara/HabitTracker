namespace HabitTracker.Application.DTOs;

public class HabitLogDto
{
    public Guid Id { get; set; }
    public Guid HabitId { get; set; }
    public DateTime LogDate { get; set; }
    public bool IsCompleted { get; set; }
    public string? Notes { get; set; }
}