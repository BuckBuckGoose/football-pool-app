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
    private readonly ILogger<ScoreGameModel> _logger;

    public ScoreGameModel(ApplicationDbContext context, ILogger<ScoreGameModel> logger)
    {
        _context = context;
        _logger = logger;
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
        _logger.LogInformation("ScoreGame OnPostAsync called. Game.Id={Id}", Game?.Id);

        // Load the existing game including picks
        var id = Game?.Id ?? 0;
        var game = await _context.Games.Include(g => g.Picks).FirstOrDefaultAsync(g => g.Id == id);
        if (game == null)
        {
            return NotFound();
        }

        // Update only the score fields from the bound Game to avoid ModelState validation
        // failing due to other [Required] properties on Game that aren't posted here.
        // Log the posted form values to help diagnose binding issues
        foreach (var key in Request.Form.Keys)
        {
            _logger.LogInformation("Form: {Key} = {Value}", key, Request.Form[key]);
        }

        // Manually bind the score fields from the form to avoid TryUpdateModelAsync binding problems
        var favValue = Request.Form["Game.FavoriteScore"].ToString();
        var undValue = Request.Form["Game.UnderdogScore"].ToString();

        if (int.TryParse(favValue, out var favScore))
        {
            game.FavoriteScore = favScore;
        }
        else
        {
            game.FavoriteScore = null;
            _logger.LogWarning("Failed to parse FavoriteScore from form value '{Value}'", favValue);
        }

        if (int.TryParse(undValue, out var undScore))
        {
            game.UnderdogScore = undScore;
        }
        else
        {
            game.UnderdogScore = null;
            _logger.LogWarning("Failed to parse UnderdogScore from form value '{Value}'", undValue);
        }

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
        _logger.LogInformation("Saved scores for Game.Id={Id}: FavoriteScore={Fav} UnderdogScore={Und}", game.Id, game.FavoriteScore, game.UnderdogScore);

        return RedirectToPage("./Games");
    }
}
