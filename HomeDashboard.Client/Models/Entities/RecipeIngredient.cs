using System.ComponentModel.DataAnnotations;

namespace HomeDashboard.Client.Models.Entities
{
    public class RecipeIngredient
    {
        public int Id { get; set; }

        // Verknüpfung zum Rezept
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        // Verknüpfung zur Zutat
        public int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }

        // Spezifische Daten für dieses EINE Rezept
        public double Amount { get; set; } // z.B. 200
        public string Unit { get; set; } = "g"; // z.B. "ml", "EL", "g"

        public string? Note { get; set; } // z.B. "fein gehackt"
    }
}
