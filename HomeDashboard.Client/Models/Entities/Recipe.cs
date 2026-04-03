using System.ComponentModel.DataAnnotations;

namespace HomeDashboard.Client.Models.Entities
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Instructions { get; set; } // Kochschritte

        public int PreparationTimeMinutes { get; set; }

        public int Servings { get; set; } = 4; // Personenanzahl

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Kategorien für die KI-Suche (z.B. "Vegan", "Schnell", "Italienisch")
        public string? Tags { get; set; }

        // Verknüpfung zu den Zutaten
        public List<RecipeIngredient> Ingredients { get; set; } = new();
    }
}
