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

    [HttpGet]
    public async Task<IActionResult> GetRecipes()
    {
        var recipes = await _context.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
        return Ok(recipes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRecipe(int id)
    {
        var recipe = await _context.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (recipe == null) return NotFound();
        return Ok(recipe);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (recipe == null) return NotFound();
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRecipe(int id, [FromBody] Recipe incoming)
    {
        var existing = await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (existing == null) return NotFound();

        existing.Title = incoming.Title;
        existing.Description = incoming.Description;
        existing.Instructions = incoming.Instructions;
        existing.PreparationTimeMinutes = incoming.PreparationTimeMinutes;
        existing.Servings = incoming.Servings;
        existing.Tags = incoming.Tags;

        _context.RecipeIngredients.RemoveRange(existing.Ingredients);

        foreach (var ri in incoming.Ingredients)
        {
            var name = ri.Ingredient?.Name?.Trim();
            if (string.IsNullOrEmpty(name)) continue;

            var ing = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());
            if (ing == null)
            {
                ing = new Ingredient { Name = name };
                _context.Ingredients.Add(ing);
                await _context.SaveChangesAsync();
            }
            existing.Ingredients.Add(new RecipeIngredient
            {
                IngredientId = ing.Id,
                Amount = ri.Amount,
                Unit = ri.Unit,
                Note = ri.Note
            });
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
}