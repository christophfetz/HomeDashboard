using HomeDashboard.Client.Models.Entities;

namespace HomeDashboard.Client.Models
{
    public class MealPlanDialogResult
    {
        public Recipe? Recipe { get; set; }
        public string Note { get; set; } = "";
        public bool Delete { get; set; }
    }
}
