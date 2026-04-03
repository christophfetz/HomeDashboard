using System.ComponentModel.DataAnnotations;

namespace HomeDashboard.Client.Models.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // z.B. "Olivenöl"

        public bool IsStandardStock { get; set; } = false; // "Immer zu Hause"

        public string? Category { get; set; } // z.B. "Gewürze", "Gemüse"

        // Hilft der KI später beim Zuordnen
        public string? PreferredUnit { get; set; } // z.B. "ml" oder "Stück"
    }
}
