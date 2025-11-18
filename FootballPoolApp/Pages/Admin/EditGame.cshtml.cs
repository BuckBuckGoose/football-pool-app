using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FootballPoolApp.Data;
using FootballPoolApp.Models;

namespace FootballPoolApp.Pages.Admin;

[Authorize(Roles = "Admin")]
public class EditGameModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditGameModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Game Game { get; set; } = new Game();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        Game = game;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var game = await _context.Games.FindAsync(Game.Id);
        if (game == null)
        {
            return NotFound();
        }

        game.Week = Game.Week;
        game.League = Game.League;
        game.Favorite = Game.Favorite;
        game.Underdog = Game.Underdog;
        game.Spread = Game.Spread;
        game.GameTime = Game.GameTime;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Games");
    }
}
