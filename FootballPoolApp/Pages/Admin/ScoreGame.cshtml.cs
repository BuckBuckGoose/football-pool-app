using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FootballPoolApp.Data;
using FootballPoolApp.Models;

namespace FootballPoolApp.Pages.Admin;

[Authorize(Roles = "Admin")]
public class ScoreGameModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ScoreGameModel(ApplicationDbContext context)
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

        var game = await _context.Games.Include(g => g.Picks).FirstOrDefaultAsync(g => g.Id == Game.Id);
        if (game == null)
        {
            return NotFound();
        }

        game.FavoriteScore = Game.FavoriteScore;
        game.UnderdogScore = Game.UnderdogScore;
        game.IsScored = true;

        // Calculate which team won against the spread
        if (game.FavoriteScore.HasValue && game.UnderdogScore.HasValue)
        {
            double favoriteMargin = game.FavoriteScore.Value - game.UnderdogScore.Value;
            bool favoriteCovered = favoriteMargin > game.Spread;

            // Update all picks for this game
            foreach (var pick in game.Picks)
            {
                if (pick.SelectedTeam == game.Favorite)
                {
                    pick.IsCorrect = favoriteCovered;
                }
                else
                {
                    pick.IsCorrect = !favoriteCovered;
                }
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToPage("./Games");
    }
}
