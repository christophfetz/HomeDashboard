using HomeDashboard.Client.Models.Entities;
using HomeDashboard.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MealPlansController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MealPlansController(ApplicationDbContext context) => _context = context;

    // GET api/mealplans?weekStart=2026-04-14
    [HttpGet]
    public async Task<IActionResult> GetWeek([FromQuery] DateTime weekStart)
    {
        var start = weekStart.Date;
        var end = start.AddDays(7);

        var plans = await _context.MealPlans
            .AsNoTracking()
            .Where(mp => mp.Date >= start && mp.Date < end)
            .Include(mp => mp.Recipe)
            .OrderBy(mp => mp.Date)
            .ThenBy(mp => mp.Type)
            .ToListAsync();

        return Ok(plans);
    }

    // POST api/mealplans
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MealPlan plan)
    {
        if (plan.RecipeId == null && string.IsNullOrWhiteSpace(plan.Note))
            return BadRequest("Entweder ein Rezept oder eine Notiz ist erforderlich.");

        plan.Date = plan.Date.Date;
        _context.MealPlans.Add(plan);
        await _context.SaveChangesAsync();

        await _context.Entry(plan).Reference(p => p.Recipe).LoadAsync();
        return Ok(plan);
    }

    // PUT api/mealplans/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MealPlan incoming)
    {
        var existing = await _context.MealPlans.FindAsync(id);
        if (existing == null) return NotFound();

        if (incoming.RecipeId == null && string.IsNullOrWhiteSpace(incoming.Note))
            return BadRequest("Entweder ein Rezept oder eine Notiz ist erforderlich.");

        existing.Date = incoming.Date.Date;
        existing.Type = incoming.Type;
        existing.RecipeId = incoming.RecipeId;
        existing.Note = incoming.Note;

        await _context.SaveChangesAsync();
        await _context.Entry(existing).Reference(p => p.Recipe).LoadAsync();
        return Ok(existing);
    }

    // DELETE api/mealplans/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var plan = await _context.MealPlans.FindAsync(id);
        if (plan == null) return NotFound();

        _context.MealPlans.Remove(plan);
        await _context.SaveChangesAsync();
        return Ok();
    }

    // DELETE api/mealplans/week?weekStart=2026-04-14
    [HttpDelete("week")]
    public async Task<IActionResult> DeleteWeek([FromQuery] DateTime weekStart)
    {
        var start = weekStart.Date;
        var end = start.AddDays(7);

        var plans = await _context.MealPlans
            .Where(mp => mp.Date >= start && mp.Date < end)
            .ToListAsync();

        if (plans.Count == 0) return Ok();

        _context.MealPlans.RemoveRange(plans);
        await _context.SaveChangesAsync();
        return Ok();
    }

    // POST api/mealplans/randomweek?weekStart=2026-04-14
    [HttpPost("randomweek")]
    public async Task<IActionResult> RandomWeek([FromQuery] DateTime weekStart)
    {
        var start = weekStart.Date;
        var end = start.AddDays(7);

        var allRecipes = await _context.Recipes
            .AsNoTracking()
            .Select(r => new { r.Id, r.Title })
            .ToListAsync();

        if (allRecipes.Count == 0)
            return BadRequest("Keine Rezepte vorhanden.");

        var existingSlots = await _context.MealPlans
            .Where(mp => mp.Date >= start && mp.Date < end
                      && (mp.Type == MealType.Dinner || mp.Type == MealType.Lunch))
            .ToListAsync();

        var usedRecipeIds = await _context.MealPlans
            .Where(mp => mp.Date >= start && mp.Date < end && mp.RecipeId != null)
            .Select(mp => mp.RecipeId!.Value)
            .Distinct()
            .ToListAsync();

        var rng = new Random();
        var pool = allRecipes
            .Where(r => !usedRecipeIds.Contains(r.Id))
            .OrderBy(_ => rng.Next())
            .ToList();

        var newPlans = new List<MealPlan>();
        foreach (var mealType in new[] { MealType.Lunch, MealType.Dinner })
        {
            for (int i = 0; i < 7; i++)
            {
                var date = start.AddDays(i);
                if (existingSlots.Any(mp => mp.Date.Date == date && mp.Type == mealType))
                    continue;

                if (pool.Count == 0)
                    pool = allRecipes.OrderBy(_ => rng.Next()).ToList();

                var chosen = pool[0];
                pool.RemoveAt(0);

                newPlans.Add(new MealPlan
                {
                    Date = date,
                    Type = mealType,
                    RecipeId = chosen.Id
                });
            }
        }

        if (newPlans.Count > 0)
        {
            _context.MealPlans.AddRange(newPlans);
            await _context.SaveChangesAsync();
        }

        var week = await _context.MealPlans
            .AsNoTracking()
            .Where(mp => mp.Date >= start && mp.Date < end)
            .Include(mp => mp.Recipe)
            .OrderBy(mp => mp.Date)
            .ThenBy(mp => mp.Type)
            .ToListAsync();

        return Ok(week);
    }
}
