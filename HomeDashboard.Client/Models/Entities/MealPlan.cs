using System.ComponentModel.DataAnnotations;

namespace HomeDashboard.Client.Models.Entities
{
    public class MealPlan
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        // Welcher Typ Mahlzeit? (Frühstück, Mittag, Abendessen)
        public MealType Type { get; set; } = MealType.Dinner;

        // Verknüpfung zum Rezept
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        // Optional: Notiz (z.B. "Gäste kommen um 19 Uhr")
        public string? Note { get; set; }
    }
    public enum MealType
    {
        Breakfast,
        Lunch,
        Dinner,
        Snack
    }
}
