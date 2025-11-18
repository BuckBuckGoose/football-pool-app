using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FootballPoolApp.Data;
using FootballPoolApp.Models;

namespace FootballPoolApp.Pages;

public class StandingsSeasonModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public StandingsSeasonModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<SeasonStanding> SeasonStandings { get; set; } = new List<SeasonStanding>();

    public async Task OnGetAsync()
    {
        var scoredGames = await _context.Games
            .Where(g => g.IsScored)
            .Select(g => g.Id)
            .ToListAsync();

        if (scoredGames.Any())
        {
            SeasonStandings = await _context.Picks
                .Where(p => scoredGames.Contains(p.GameId) && p.IsCorrect.HasValue)
                .GroupBy(p => new { p.UserId, p.User.UserName })
                .Select(g => new SeasonStanding
                {
                    UserName = g.Key.UserName ?? "Unknown",
                    TotalCorrectPicks = g.Count(p => p.IsCorrect == true),
                    TotalPicks = g.Count(),
                    Percentage = g.Count() > 0 ? (double)g.Count(p => p.IsCorrect == true) / g.Count() : 0
                })
                .OrderByDescending(s => s.Percentage)
                .ThenByDescending(s => s.TotalCorrectPicks)
                .ToListAsync();
        }
    }
}
