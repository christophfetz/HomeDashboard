using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeDashboard.Data;
using HomeDashboard.Client.Models.Entities;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IngredientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public IngredientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetIngredients()
    {
        var ingredients = await _context.Ingredients
            .AsNoTracking()
            .OrderBy(i => i.Name)
            .ToListAsync();
        return Ok(ingredients);
    }

    [HttpPost]
    public async Task<IActionResult> CreateIngredient([FromBody] Ingredient ingredient)
    {
        var name = ingredient.Name?.Trim();
        if (string.IsNullOrEmpty(name))
            return BadRequest("Name darf nicht leer sein.");

        var exists = await _context.Ingredients
            .AnyAsync(i => i.Name.ToLower() == name.ToLower());
        if (exists)
            return Conflict("Eine Zutat mit diesem Namen existiert bereits.");

        ingredient.Name = name;
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();
        return Ok(ingredient);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateIngredient(int id, [FromBody] Ingredient incoming)
    {
        var existing = await _context.Ingredients.FindAsync(id);
        if (existing == null) return NotFound();

        var name = incoming.Name?.Trim();
        if (string.IsNullOrEmpty(name))
            return BadRequest("Name darf nicht leer sein.");

        var duplicate = await _context.Ingredients
            .AnyAsync(i => i.Name.ToLower() == name.ToLower() && i.Id != id);
        if (duplicate)
            return Conflict("Eine Zutat mit diesem Namen existiert bereits.");

        existing.Name = name;
        existing.Category = incoming.Category;
        existing.PreferredUnit = incoming.PreferredUnit;
        existing.IsStandardStock = incoming.IsStandardStock;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null) return NotFound();

        var usedInRecipe = await _context.RecipeIngredients
            .AnyAsync(ri => ri.IngredientId == id);
        if (usedInRecipe)
            return Conflict("Diese Zutat wird in einem oder mehreren Rezepten verwendet und kann nicht gelöscht werden.");

        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
