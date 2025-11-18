using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FootballPoolApp.Data;
using FootballPoolApp.Models;

namespace FootballPoolApp.Pages;

public class StandingsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public StandingsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<WeeklyStanding> WeeklyStandings { get; set; } = new List<WeeklyStanding>();
    public int CurrentWeek { get; set; }
    public int MaxWeek { get; set; }
    public string ViewType { get; set; } = "weekly";

    public async Task OnGetAsync(int? week)
    {
        MaxWeek = await _context.Games.Select(g => g.Week).DefaultIfEmpty(0).MaxAsync();
        CurrentWeek = week ?? MaxWeek;
        
        if (CurrentWeek < 1) CurrentWeek = 1;

        var scoredGames = await _context.Games
            .Where(g => g.Week == CurrentWeek && g.IsScored)
            .Select(g => g.Id)
            .ToListAsync();

        if (scoredGames.Any())
        {
            WeeklyStandings = await _context.Picks
                .Where(p => scoredGames.Contains(p.GameId) && p.IsCorrect.HasValue)
                .GroupBy(p => new { p.UserId, p.User.UserName })
                .Select(g => new WeeklyStanding
                {
                    UserName = g.Key.UserName ?? "Unknown",
                    Week = CurrentWeek,
                    CorrectPicks = g.Count(p => p.IsCorrect == true),
                    TotalPicks = g.Count(),
                    Percentage = g.Count() > 0 ? (double)g.Count(p => p.IsCorrect == true) / g.Count() : 0
                })
                .OrderByDescending(s => s.Percentage)
                .ThenByDescending(s => s.CorrectPicks)
                .ToListAsync();
        }
    }
}
