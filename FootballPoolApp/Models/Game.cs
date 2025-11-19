using System.ComponentModel.DataAnnotations;

namespace FootballPoolApp.Models;

public class Game
{
    public int Id { get; set; }

    [Required]
    public int Week { get; set; }

    [Required]
    [StringLength(10)]
    public string League { get; set; } = string.Empty; // "NCAAF" or "NFL"

    [Required]
    [StringLength(100)]
    public string Favorite { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Underdog { get; set; } = string.Empty;

    [Required]
    public double Spread { get; set; }

    public DateTime GameTime { get; set; }

    public int? FavoriteScore { get; set; }
    
    public int? UnderdogScore { get; set; }

    public bool IsScored { get; set; } = false;

    // Navigation property
    public ICollection<Pick> Picks { get; set; } = new List<Pick>();
}
