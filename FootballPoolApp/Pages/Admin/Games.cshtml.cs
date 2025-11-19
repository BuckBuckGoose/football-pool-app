using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FootballPoolApp.Data;
using FootballPoolApp.Models;

namespace FootballPoolApp.Pages.Admin;

[Authorize(Roles = "Admin")]
public class GamesModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public GamesModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Game> Games { get; set; } = new List<Game>();

    public async Task OnGetAsync()
    {
        Games = await _context.Games
            .OrderBy(g => g.Week)
            .ThenBy(g => g.GameTime)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}
