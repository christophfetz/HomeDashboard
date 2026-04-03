using HomeDashboard.Client.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeDashboard.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Konfiguration: Ein Rezept hat viele RecipeIngredients
            builder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.Ingredients)
                .HasForeignKey(ri => ri.RecipeId);

            // Konfiguration: Eine Zutat kann in vielen RecipeIngredients vorkommen
            builder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany() // Wir brauchen keine Liste von Rezepten in der Zutat-Klasse, außer du willst es
                .HasForeignKey(ri => ri.IngredientId);

            // Optional: Einzigartiger Name für Zutaten, um Dubletten zu vermeiden
            builder.Entity<Ingredient>()
                .HasIndex(i => i.Name)
                .IsUnique();

            // Konfiguration für den Wochenplan
            builder.Entity<MealPlan>()
                .HasOne(mp => mp.Recipe)
                .WithMany() // Ein Rezept kann in vielen MealPlans auftauchen
                .HasForeignKey(mp => mp.RecipeId)
                .OnDelete(DeleteBehavior.Cascade); // Wenn Rezept weg, dann auch aus Wochenplan raus

            // Index für ShoppingItems (optional, macht die Suche schneller)
            builder.Entity<ShoppingItem>()
                .HasIndex(s => s.Name);
        }
    }
}
