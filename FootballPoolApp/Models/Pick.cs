using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FootballPoolApp.Models;

public class Pick
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public int GameId { get; set; }

    [Required]
    [StringLength(100)]
    public string SelectedTeam { get; set; } = string.Empty; // Either Favorite or Underdog

    public bool? IsCorrect { get; set; } // Null until game is scored

    public DateTime PickedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public IdentityUser User { get; set; } = null!;
    public Game Game { get; set; } = null!;
}
