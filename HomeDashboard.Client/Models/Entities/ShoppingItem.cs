using System.ComponentModel.DataAnnotations;

namespace HomeDashboard.Client.Models.Entities
{
    public class ShoppingItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public double Amount { get; set; }

        public string Unit { get; set; } = string.Empty; // z.B. "Packung", "g", "Stück"

        // Erlaubt es, Dinge abzuhaken
        public bool IsChecked { get; set; } = false;

        // Hilfreich für die Sortierung im Laden
        public string? Category { get; set; } // z.B. "Obst & Gemüse", "Kühlregal"

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Falls das Item aus einem konkreten Rezept stammt (optional)
        public int? RelatedRecipeId { get; set; }
    }
}
