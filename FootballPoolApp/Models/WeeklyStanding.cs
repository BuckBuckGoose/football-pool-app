namespace FootballPoolApp.Models;

public class WeeklyStanding
{
    public string UserName { get; set; } = string.Empty;
    public int Week { get; set; }
    public int CorrectPicks { get; set; }
    public int TotalPicks { get; set; }
    public double Percentage { get; set; }
}
