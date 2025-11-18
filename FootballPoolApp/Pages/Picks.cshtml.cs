using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FootballPoolApp.Data;
using FootballPoolApp.Models;

namespace FootballPoolApp.Pages;

[Authorize]
public class PicksModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public PicksModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public List<Game> Games { get; set; } = new List<Game>();
    public List<Pick> UserPicks { get; set; } = new List<Pick>();
    public int CurrentWeek { get; set; }
    public int MaxWeek { get; set; }
    public string Message { get; set; } = string.Empty;

    [BindProperty]
    public List<PickInput> Picks { get; set; } = new List<PickInput>();

    public class PickInput
    {
        public int GameId { get; set; }
        public string? SelectedTeam { get; set; }
    }

    public async Task OnGetAsync(int? week)
    {
        var weeks = await _context.Games.Select(g => g.Week).ToListAsync();
        MaxWeek = weeks.Any() ? weeks.Max() : 1;
        CurrentWeek = week ?? MaxWeek;
        
        if (CurrentWeek < 1) CurrentWeek = 1;

        Games = await _context.Games
            .Where(g => g.Week == CurrentWeek)
            .OrderBy(g => g.GameTime)
            .ToListAsync();

        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            UserPicks = await _context.Picks
                .Where(p => p.UserId == user.Id && p.Game.Week == CurrentWeek)
                .ToListAsync();
        }
    }

    public async Task<IActionResult> OnPostAsync(int week)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        foreach (var pickInput in Picks.Where(p => !string.IsNullOrEmpty(p.SelectedTeam)))
        {
            var game = await _context.Games.FindAsync(pickInput.GameId);
            if (game == null || game.GameTime <= DateTime.UtcNow)
            {
                continue; // Skip locked games
            }

            var existingPick = await _context.Picks
                .FirstOrDefaultAsync(p => p.UserId == user.Id && p.GameId == pickInput.GameId);

            if (existingPick != null)
            {
                existingPick.SelectedTeam = pickInput.SelectedTeam!;
                existingPick.PickedAt = DateTime.UtcNow;
            }
            else
            {
                _context.Picks.Add(new Pick
                {
                    UserId = user.Id,
                    GameId = pickInput.GameId,
                    SelectedTeam = pickInput.SelectedTeam!,
                    PickedAt = DateTime.UtcNow
                });
            }
        }

        await _context.SaveChangesAsync();
        Message = "Your picks have been saved!";
        
        return RedirectToPage(new { week });
    }
}
