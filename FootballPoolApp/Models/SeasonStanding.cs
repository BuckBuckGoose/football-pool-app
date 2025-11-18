namespace FootballPoolApp.Models;

public class SeasonStanding
{
    public string UserName { get; set; } = string.Empty;
    public int TotalCorrectPicks { get; set; }
    public int TotalPicks { get; set; }
    public double Percentage { get; set; }
}
