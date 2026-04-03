using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeDashboard.Data;
using HomeDashboard.Client.Models.Entities; // Pfad zu deinen verschobenen Models
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecipesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RecipesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecipe([FromBody] Recipe recipe)
    {
        // Automatische Zutaten-Logik auf dem Server
        foreach (var ri in recipe.Ingredients)
        {
            // Wir suchen die Zutat im Stamm (Ingredient) anhand des Namens
            // WICHTIG: Hier nutzen wir den Namen aus der Navigation Property, falls vorhanden
            var ingredientName = ri.Ingredient?.Name?.Trim();
            if (string.IsNullOrEmpty(ingredientName)) continue;

            var existing = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.Name.ToLower() == ingredientName.ToLower());

            if (existing == null)
            {
                existing = new Ingredient { Name = ingredientName };
                _context.Ingredients.Add(existing);
                await _context.SaveChangesAsync();
            }

            ri.IngredientId = existing.Id;
            ri.Ingredient = null; // Wir setzen die Navigation Property auf null für den Save
        }

        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();
        return Ok();
    }
}