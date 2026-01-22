namespace HabitTracker.Application.DTOs;

public class HabitStatsDto
{
    public Guid HabitId { get; set; }
    public string HabitName { get; set; } = string.Empty;
    public int CurrentStreak { get; set; }
    public int TotalCompletions { get; set; }
    public double CompletionRate { get; set; }
}