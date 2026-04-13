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

            // Konfiguration: MealPlan hat optionale Verknüpfung zum Rezept
            builder.Entity<MealPlan>()
                .HasOne(mp => mp.Recipe)
                .WithMany()
                .HasForeignKey(mp => mp.RecipeId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // Seed: Grundzutaten-Liste
            builder.Entity<Ingredient>().HasData(
                // Gewürze & Kräuter
                new Ingredient { Id = 1,  Name = "Salz",                    Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "Prise" },
                new Ingredient { Id = 2,  Name = "Pfeffer",                 Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "Prise" },
                new Ingredient { Id = 3,  Name = "Paprikapulver (edelsüß)", Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 4,  Name = "Paprikapulver (scharf)",  Category = "Gewürze & Kräuter", IsStandardStock = false, PreferredUnit = "TL" },
                new Ingredient { Id = 5,  Name = "Kümmel",                  Category = "Gewürze & Kräuter", IsStandardStock = false, PreferredUnit = "TL" },
                new Ingredient { Id = 6,  Name = "Muskatnuss",              Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "Prise" },
                new Ingredient { Id = 7,  Name = "Zimt",                    Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 8,  Name = "Kreuzkümmel",             Category = "Gewürze & Kräuter", IsStandardStock = false, PreferredUnit = "TL" },
                new Ingredient { Id = 9,  Name = "Thymian (getrocknet)",    Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 10, Name = "Oregano (getrocknet)",    Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 11, Name = "Basilikum (getrocknet)",  Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 12, Name = "Lorbeerblatt",            Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "Stück" },
                new Ingredient { Id = 13, Name = "Curry",                   Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 14, Name = "Kurkuma",                 Category = "Gewürze & Kräuter", IsStandardStock = false, PreferredUnit = "TL" },
                new Ingredient { Id = 15, Name = "Chilipulver",             Category = "Gewürze & Kräuter", IsStandardStock = false, PreferredUnit = "TL" },
                new Ingredient { Id = 16, Name = "Knoblauchpulver",         Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 17, Name = "Zwiebelpulver",           Category = "Gewürze & Kräuter", IsStandardStock = false, PreferredUnit = "TL" },
                new Ingredient { Id = 18, Name = "Petersilie (getrocknet)", Category = "Gewürze & Kräuter", IsStandardStock = true,  PreferredUnit = "TL" },
                // Öle & Fette
                new Ingredient { Id = 19, Name = "Olivenöl",       Category = "Öle & Fette", IsStandardStock = true,  PreferredUnit = "ml" },
                new Ingredient { Id = 20, Name = "Sonnenblumenöl",  Category = "Öle & Fette", IsStandardStock = true,  PreferredUnit = "ml" },
                new Ingredient { Id = 21, Name = "Butter",          Category = "Öle & Fette", IsStandardStock = true,  PreferredUnit = "g" },
                new Ingredient { Id = 22, Name = "Margarine",       Category = "Öle & Fette", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 23, Name = "Schmalz",         Category = "Öle & Fette", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 24, Name = "Rapsöl",          Category = "Öle & Fette", IsStandardStock = true,  PreferredUnit = "ml" },
                // Backen
                new Ingredient { Id = 25, Name = "Mehl (Typ 405)",   Category = "Backen", IsStandardStock = true,  PreferredUnit = "g" },
                new Ingredient { Id = 26, Name = "Mehl (Typ 550)",   Category = "Backen", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 27, Name = "Zucker",           Category = "Backen", IsStandardStock = true,  PreferredUnit = "g" },
                new Ingredient { Id = 28, Name = "Brauner Zucker",   Category = "Backen", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 29, Name = "Puderzucker",      Category = "Backen", IsStandardStock = true,  PreferredUnit = "g" },
                new Ingredient { Id = 30, Name = "Backpulver",       Category = "Backen", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 31, Name = "Natron",           Category = "Backen", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 32, Name = "Hefe (trocken)",   Category = "Backen", IsStandardStock = true,  PreferredUnit = "Päckchen" },
                new Ingredient { Id = 33, Name = "Vanilleextrakt",   Category = "Backen", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 34, Name = "Kakao (ungesüßt)", Category = "Backen", IsStandardStock = true,  PreferredUnit = "EL" },
                new Ingredient { Id = 35, Name = "Speisestärke",     Category = "Backen", IsStandardStock = true,  PreferredUnit = "EL" },
                new Ingredient { Id = 36, Name = "Paniermehl",       Category = "Backen", IsStandardStock = true,  PreferredUnit = "g" },
                // Nudeln & Reis
                new Ingredient { Id = 37, Name = "Spaghetti",         Category = "Nudeln & Reis", IsStandardStock = true,  PreferredUnit = "g" },
                new Ingredient { Id = 38, Name = "Penne",             Category = "Nudeln & Reis", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 39, Name = "Tagliatelle",       Category = "Nudeln & Reis", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 40, Name = "Reis (Langkorn)",   Category = "Nudeln & Reis", IsStandardStock = true,  PreferredUnit = "g" },
                new Ingredient { Id = 41, Name = "Reis (Risotto)",    Category = "Nudeln & Reis", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 42, Name = "Nudeln (Hörnchen)", Category = "Nudeln & Reis", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 43, Name = "Couscous",          Category = "Nudeln & Reis", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 44, Name = "Linsen (rot)",      Category = "Nudeln & Reis", IsStandardStock = false, PreferredUnit = "g" },
                // Konserven
                new Ingredient { Id = 45, Name = "Kichererbsen (Dose)",     Category = "Konserven", IsStandardStock = false, PreferredUnit = "Dose" },
                new Ingredient { Id = 46, Name = "Tomaten (gehackt, Dose)", Category = "Konserven", IsStandardStock = true,  PreferredUnit = "Dose" },
                new Ingredient { Id = 47, Name = "Tomatenmark",             Category = "Konserven", IsStandardStock = true,  PreferredUnit = "EL" },
                new Ingredient { Id = 48, Name = "Kidneybohnen (Dose)",     Category = "Konserven", IsStandardStock = false, PreferredUnit = "Dose" },
                new Ingredient { Id = 49, Name = "Mais (Dose)",             Category = "Konserven", IsStandardStock = false, PreferredUnit = "Dose" },
                new Ingredient { Id = 50, Name = "Thunfisch (Dose)",        Category = "Konserven", IsStandardStock = false, PreferredUnit = "Dose" },
                new Ingredient { Id = 51, Name = "Sardinen (Dose)",         Category = "Konserven", IsStandardStock = false, PreferredUnit = "Dose" },
                new Ingredient { Id = 52, Name = "Kokosmilch (Dose)",       Category = "Konserven", IsStandardStock = false, PreferredUnit = "Dose" },
                // Milchprodukte
                new Ingredient { Id = 53, Name = "Milch (3,5%)",    Category = "Milchprodukte", IsStandardStock = true,  PreferredUnit = "ml" },
                new Ingredient { Id = 54, Name = "Sahne",           Category = "Milchprodukte", IsStandardStock = false, PreferredUnit = "ml" },
                new Ingredient { Id = 55, Name = "Saure Sahne",     Category = "Milchprodukte", IsStandardStock = false, PreferredUnit = "ml" },
                new Ingredient { Id = 56, Name = "Crème fraîche",   Category = "Milchprodukte", IsStandardStock = false, PreferredUnit = "ml" },
                new Ingredient { Id = 57, Name = "Joghurt (natur)", Category = "Milchprodukte", IsStandardStock = true,  PreferredUnit = "g" },
                new Ingredient { Id = 58, Name = "Quark",           Category = "Milchprodukte", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 59, Name = "Käse (gerieben)", Category = "Milchprodukte", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 60, Name = "Parmesan",        Category = "Milchprodukte", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 61, Name = "Eier",            Category = "Milchprodukte", IsStandardStock = true,  PreferredUnit = "Stück" },
                // Gemüse
                new Ingredient { Id = 62, Name = "Zwiebel",             Category = "Gemüse", IsStandardStock = true,  PreferredUnit = "Stück" },
                new Ingredient { Id = 63, Name = "Knoblauch",           Category = "Gemüse", IsStandardStock = true,  PreferredUnit = "Stück" },
                new Ingredient { Id = 64, Name = "Karotte",             Category = "Gemüse", IsStandardStock = true,  PreferredUnit = "Stück" },
                new Ingredient { Id = 65, Name = "Kartoffel",           Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 66, Name = "Tomate",              Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 67, Name = "Paprika (rot)",       Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 68, Name = "Paprika (gelb)",      Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 69, Name = "Zucchini",            Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 70, Name = "Aubergine",           Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 71, Name = "Lauch",               Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 72, Name = "Sellerie",            Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 73, Name = "Spinat (frisch)",     Category = "Gemüse", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 74, Name = "Brokkoli",            Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 75, Name = "Blumenkohl",          Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 76, Name = "Petersilie (frisch)", Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Bund" },
                new Ingredient { Id = 77, Name = "Schnittlauch",        Category = "Gemüse", IsStandardStock = false, PreferredUnit = "Bund" },
                // Obst
                new Ingredient { Id = 78, Name = "Zitrone", Category = "Obst", IsStandardStock = true,  PreferredUnit = "Stück" },
                new Ingredient { Id = 79, Name = "Apfel",   Category = "Obst", IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 80, Name = "Banane",  Category = "Obst", IsStandardStock = false, PreferredUnit = "Stück" },
                // Fleisch & Fisch
                new Ingredient { Id = 81, Name = "Hühnerbrust",           Category = "Fleisch & Fisch", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 82, Name = "Hackfleisch (gemischt)", Category = "Fleisch & Fisch", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 83, Name = "Speck (Würfel)",         Category = "Fleisch & Fisch", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 84, Name = "Lachs (Filet)",          Category = "Fleisch & Fisch", IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 85, Name = "Rindfleisch (Gulasch)",  Category = "Fleisch & Fisch", IsStandardStock = false, PreferredUnit = "g" },
                // Saucen & Würzmittel
                new Ingredient { Id = 86, Name = "Sojasauce",        Category = "Saucen & Würzmittel", IsStandardStock = true,  PreferredUnit = "EL" },
                new Ingredient { Id = 87, Name = "Senf",             Category = "Saucen & Würzmittel", IsStandardStock = true,  PreferredUnit = "TL" },
                new Ingredient { Id = 88, Name = "Ketchup",          Category = "Saucen & Würzmittel", IsStandardStock = true,  PreferredUnit = "EL" },
                new Ingredient { Id = 89, Name = "Essig (Weißwein)", Category = "Saucen & Würzmittel", IsStandardStock = true,  PreferredUnit = "EL" },
                new Ingredient { Id = 90, Name = "Brühe (Gemüse)",   Category = "Saucen & Würzmittel", IsStandardStock = true,  PreferredUnit = "ml" },
                // Zusätzliche Zutaten für österreichische Rezepte
                new Ingredient { Id = 91,  Name = "Kalbsschnitzel",    Category = "Fleisch & Fisch",   IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 92,  Name = "Rotwein",            Category = "Getränke",          IsStandardStock = false, PreferredUnit = "ml" },
                new Ingredient { Id = 93,  Name = "Tafelspitz",         Category = "Fleisch & Fisch",   IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 94,  Name = "Kren (Meerrettich)", Category = "Gewürze & Kräuter", IsStandardStock = false, PreferredUnit = "EL" },
                new Ingredient { Id = 95,  Name = "Strudelteig",        Category = "Backen",            IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 96,  Name = "Semmel",             Category = "Brot & Backwaren",  IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 97,  Name = "Weißkraut",          Category = "Gemüse",            IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 98,  Name = "Fleckerl-Nudeln",    Category = "Nudeln & Reis",     IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 99,  Name = "Grieß",              Category = "Backen",            IsStandardStock = false, PreferredUnit = "g" },
                new Ingredient { Id = 100, Name = "Zwetschken",         Category = "Obst",              IsStandardStock = false, PreferredUnit = "Stück" },
                new Ingredient { Id = 101, Name = "Rindsleber",         Category = "Fleisch & Fisch",   IsStandardStock = false, PreferredUnit = "g" }
            );

            // Seed: 20 gut bürgerliche österreichische Rezepte
            builder.Entity<Recipe>().HasData(
                new Recipe { Id = 1,  Title = "Wiener Schnitzel",       Description = "Das Klassiker schlechthin – zartes Kalbsschnitzel, goldbraun paniert.", Instructions = "1. Schnitzel dünn klopfen und salzen.\n2. Nacheinander in Mehl, verquirlten Eiern und Paniermehl wenden.\n3. In heißem Schmalz schwimmend goldbraun backen (je 2–3 Min. pro Seite).\n4. Auf Küchenpapier abtropfen lassen.\n5. Mit Zitronenspalte servieren.", PreparationTimeMinutes = 30, Servings = 4, Tags = "Österreich,Klassiker,Fleisch",         CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 2,  Title = "Rindsgulasch",           Description = "Würziger Rindseintopf mit viel Zwiebel und Paprika – typisch österreichisch.", Instructions = "1. Zwiebeln in Schmalz goldbraun rösten.\n2. Paprikapulver einrühren, kurz mitrösten.\n3. Fleisch in Würfel schneiden und anbraten.\n4. Tomatenmark, Rotwein und Brühe zugeben.\n5. Mit Kümmel, Lorbeer würzen.\n6. Zugedeckt ca. 90 Min. weich schmoren.", PreparationTimeMinutes = 120, Servings = 4, Tags = "Österreich,Klassiker,Fleisch,Eintopf",     CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 3,  Title = "Wiener Tafelspitz",      Description = "Zart gekochtes Rindfleisch in aromatischer Brühe – das Sonntagsgericht der Wiener.", Instructions = "1. Zwiebel halbieren und in trockenem Topf anrösten.\n2. Kaltes Wasser aufkochen, Fleisch einlegen.\n3. Gemüse (Karotte, Sellerie, Lauch) und Gewürze zugeben.\n4. 2 Stunden bei kleiner Hitze kochen.\n5. Fleisch aufschneiden, mit Kren und Brühe servieren.", PreparationTimeMinutes = 150, Servings = 4, Tags = "Österreich,Klassiker,Fleisch,Suppe",       CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 4,  Title = "Backhendl",              Description = "Knusprig paniertes Hendl – der österreichische Klassiker für Biergärten und Feste.", Instructions = "1. Hühnerbrust in mundgerechte Stücke schneiden und salzen.\n2. In Mehl, verquirlten Eiern und Paniermehl wenden.\n3. In heißem Öl schwimmend goldbraun frittieren (ca. 6–8 Min.).\n4. Auf Küchenpapier abtropfen lassen.\n5. Mit Zitrone und Erdäpfelsalat servieren.", PreparationTimeMinutes = 50, Servings = 4, Tags = "Österreich,Klassiker,Geflügel,Paniert",    CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 5,  Title = "Zwiebelrostbraten",      Description = "Saftiger Rindsbraten mit knusprig-karamellisierten Röstwiebeln.", Instructions = "1. Rindfleisch in ca. 2 cm dicke Scheiben schneiden, salzen und pfeffern.\n2. Zwiebeln in feine Ringe schneiden.\n3. Schnitzel in heißem Öl von beiden Seiten scharf anbraten.\n4. Zwiebeln separat in Butter goldbraun rösten.\n5. Fleisch mit Röstwiebeln bedecken und servieren.", PreparationTimeMinutes = 40, Servings = 4, Tags = "Österreich,Klassiker,Fleisch,Rindfleisch",  CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 6,  Title = "Kaiserschmarrn",         Description = "Fluffiger zerrissener Pfannkuchen mit Puderzucker – die Leibspeise von Kaiser Franz Joseph.", Instructions = "1. Eier trennen. Eigelbe mit Milch, Mehl, Zucker und Vanille verrühren.\n2. Eiweiß zu steifem Schnee schlagen und unterheben.\n3. Butter in Pfanne erhitzen, Teig eingießen.\n4. Wenn Unterseite goldbraun, wenden und in Stücke reißen.\n5. Mit Puderzucker bestäuben und mit Zwetschkenröster servieren.", PreparationTimeMinutes = 25, Servings = 4, Tags = "Österreich,Süßspeise,Dessert,Klassiker",    CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 7,  Title = "Apfelstrudel",           Description = "Hauchdünner Strudelteig mit würziger Apfelfüllung – ein österreichisches Meisterstück.", Instructions = "1. Äpfel schälen, entkernen und in dünne Scheiben schneiden.\n2. Mit Zucker und Zimt vermischen.\n3. Strudelteig ausbreiten, mit flüssiger Butter bestreichen.\n4. Gebräuntes Paniermehl auftragen, Apfelfüllung verteilen.\n5. Einrollen und bei 180°C ca. 35 Min. goldbraun backen.\n6. Mit Puderzucker bestäuben.", PreparationTimeMinutes = 60, Servings = 6, Tags = "Österreich,Süßspeise,Dessert,Backen",       CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 8,  Title = "Semmelknödel",           Description = "Saftige Knödel aus alten Semmeln – die perfekte Beilage zu Gulasch und Braten.", Instructions = "1. Semmeln in Würfel schneiden, mit warmer Milch übergießen, 15 Min. ziehen lassen.\n2. Zwiebel in Butter glasig dünsten.\n3. Mit Eiern, Petersilie, Salz und Muskatnuss vermengen.\n4. Aus der Masse Knödel formen.\n5. In leicht wallendem Salzwasser ca. 15 Min. garen.", PreparationTimeMinutes = 45, Servings = 4, Tags = "Österreich,Beilage,Vegetarisch,Klassiker",  CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 9,  Title = "Palatschinken",          Description = "Dünne, zarte Pfannkuchen – in Österreich zum Frühstück, als Dessert oder herzhaft gefüllt.", Instructions = "1. Mehl, Eier, Milch, Zucker und Salz zu einem glatten Teig verrühren.\n2. 30 Min. rasten lassen.\n3. Butter in Pfanne erhitzen, dünne Palatschinken backen.\n4. Nach Belieben mit Marmelade, Topfen oder Schlagobers füllen und einrollen.", PreparationTimeMinutes = 30, Servings = 4, Tags = "Österreich,Süßspeise,Frühstück,Vegetarisch", CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 10, Title = "Krautfleckerl",          Description = "Hausgemachte Fleckerl-Nudeln mit karamellisiertem Weißkraut – einfach und köstlich.", Instructions = "1. Weißkraut in Streifen schneiden, salzen und etwas stehen lassen.\n2. Zwiebel in Butter goldbraun rösten, Zucker zugeben und karamellisieren.\n3. Kraut ausdrücken und mitrösten, mit Kümmel würzen.\n4. Fleckerl al dente kochen, unterheben.\n5. Mit Saurer Sahne verfeinern.", PreparationTimeMinutes = 45, Servings = 4, Tags = "Österreich,Vegetarisch,Klassiker,Nudeln",    CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 11, Title = "Grießnockerlsuppe",      Description = "Zarte Grießnockerl in goldener Rindsuppe – der Inbegriff österreichischer Hausmannskost.", Instructions = "1. Weiche Butter cremig rühren, Ei und Grieß einarbeiten.\n2. Mit Salz und Muskat würzen.\n3. Masse 30 Min. kühlen.\n4. Mit feuchten Händen Nockerl formen.\n5. In siedende Brühe einlegen und 15 Min. ziehen lassen.\n6. Mit frischer Petersilie bestreuen.", PreparationTimeMinutes = 50, Servings = 4, Tags = "Österreich,Suppe,Vegetarisch,Klassiker",    CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 12, Title = "Linsensuppe mit Speck",  Description = "Herzhafte Linsensuppe mit geräuchertem Speck – warm und sättigend.", Instructions = "1. Speck in Topf ausbraten, Zwiebeln und Gemüse mitdünsten.\n2. Linsen zugeben und kurz mitrösten.\n3. Mit Brühe aufgießen.\n4. Lorbeerblätter zugeben, ca. 30 Min. köcheln.\n5. Mit Essig abschmecken, salzen und pfeffern.", PreparationTimeMinutes = 50, Servings = 4, Tags = "Österreich,Suppe,Eintopf,Hülsenfrüchte",   CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 13, Title = "Erdäpfelgulasch",        Description = "Würziges Kartoffelgulasch – ein einfaches, vegetarisches Alltagsgericht.", Instructions = "1. Zwiebeln in Schmalz goldbraun rösten.\n2. Paprikapulver und Kümmel einrühren.\n3. Knoblauch und Tomatenmark zugeben.\n4. Kartoffeln in Würfel schneiden und unterheben.\n5. Mit Brühe aufgießen, 25 Min. weich köcheln.\n6. Mit Salz und Pfeffer abschmecken.", PreparationTimeMinutes = 45, Servings = 4, Tags = "Österreich,Vegetarisch,Eintopf,Kartoffel",  CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 14, Title = "Zwetschkenknödel",       Description = "Kartoffelteig-Knödel mit ganzer Zwetschke – ein süßes Hauptgericht der österreichischen Küche.", Instructions = "1. Kartoffeln kochen, schälen und pressen.\n2. Mit Mehl, Ei und Butter zu einem Teig verkneten.\n3. Teig portionieren, Zwetschke einwickeln und Knödel formen.\n4. In leicht wallendem Wasser ca. 15 Min. garen.\n5. In gebräuntem Paniermehl wälzen, mit Puderzucker und Zimt bestäuben.", PreparationTimeMinutes = 60, Servings = 4, Tags = "Österreich,Süßspeise,Vegetarisch,Dessert",  CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 15, Title = "Faschierter Braten",     Description = "Saftiger Hackbraten nach österreichischer Art – ein echtes Sonntagsgericht.", Instructions = "1. Semmeln einweichen und ausdrücken.\n2. Hackfleisch mit Semmeln, Eiern, Zwiebel, Petersilie, Senf, Salz und Pfeffer vermengen.\n3. Zu einem Laib formen, mit Speckstreifen belegen.\n4. Bei 180°C ca. 60 Min. im Ofen backen.\n5. Kurz ruhen lassen und aufschneiden.", PreparationTimeMinutes = 90, Servings = 6, Tags = "Österreich,Fleisch,Braten,Klassiker",       CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 16, Title = "Gebackener Blumenkohl",  Description = "Blumenkohlröschen in knuspriger Panade – einfach, günstig und köstlich.", Instructions = "1. Blumenkohl in Röschen teilen und in Salzwasser 5 Min. blanchieren.\n2. Abkühlen lassen.\n3. In Mehl, Ei und Paniermehl wenden.\n4. In heißem Öl goldbraun frittieren.\n5. Mit Zitronensaft beträufeln und servieren.", PreparationTimeMinutes = 35, Servings = 4, Tags = "Österreich,Vegetarisch,Paniert,Gemüse",     CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 17, Title = "Penne mit Tomatensauce", Description = "Klassische Tomatensauce mit Nudeln – schnell, einfach und immer gut.", Instructions = "1. Zwiebel und Knoblauch in Olivenöl glasig dünsten.\n2. Dosentomaten zugeben, mit Zucker, Basilikum und Oregano würzen.\n3. 20 Min. einkochen lassen.\n4. Penne al dente kochen.\n5. Sauce mit Salz und Pfeffer abschmecken, über Nudeln geben.", PreparationTimeMinutes = 30, Servings = 4, Tags = "Nudeln,Vegetarisch,Schnell,Tomatensauce",   CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 18, Title = "Spinatknödel",           Description = "Knödel mit frischem Spinat und Parmesan – eine leichte und schmackhafte Mahlzeit.", Instructions = "1. Spinat waschen, blanchieren, ausdrücken und grob hacken.\n2. Semmeln in Milch einweichen.\n3. Zwiebel in Butter anschwitzen.\n4. Alle Zutaten vermengen, mit Muskat würzen.\n5. Knödel formen und in Salzwasser 15 Min. garen.\n6. Mit gebräunter Butter und Parmesan servieren.", PreparationTimeMinutes = 50, Servings = 4, Tags = "Vegetarisch,Knödel,Spinat,Österreich",      CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 19, Title = "Leberknödelsuppe",       Description = "Deftige Leberknödel in klarer Rindsuppe – ein Klassiker der Wiener Küche.", Instructions = "1. Semmeln einweichen und ausdrücken.\n2. Leber fein hacken oder faschieren.\n3. Mit Semmeln, Ei, Zwiebel, Petersilie, Salz und Muskat vermengen.\n4. Knödel formen und in siedender Brühe 15 Min. garen.\n5. Mit Schnittlauch bestreut servieren.", PreparationTimeMinutes = 55, Servings = 4, Tags = "Österreich,Suppe,Klassiker,Innereien",      CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Recipe { Id = 20, Title = "Topfenstrudel",          Description = "Cremiger Topfenstrudel im knusprigen Strudelteig – ein österreichischer Kaffeehausklassiker.", Instructions = "1. Topfen mit Eiern, Zucker, Vanilleextrakt und Zitronenschale verrühren.\n2. Strudelteig ausbreiten, mit Butter bestreichen.\n3. Topfenfüllung gleichmäßig auftragen und einrollen.\n4. Bei 180°C ca. 35 Min. goldbraun backen.\n5. Warm mit Puderzucker bestäuben servieren.", PreparationTimeMinutes = 60, Servings = 6, Tags = "Österreich,Süßspeise,Dessert,Backen,Topfen",CreatedAt = new DateTime(2026, 4, 12, 0, 0, 0, DateTimeKind.Utc) }
            );

            // Seed: Rezept-Zutaten-Verknüpfungen
            builder.Entity<RecipeIngredient>().HasData(
                // Wiener Schnitzel (Recipe 1)
                new RecipeIngredient { Id = 1,   RecipeId = 1,  IngredientId = 91, Amount = 600,  Unit = "g",     Note = "dünn geklopft" },
                new RecipeIngredient { Id = 2,   RecipeId = 1,  IngredientId = 25, Amount = 100,  Unit = "g" },
                new RecipeIngredient { Id = 3,   RecipeId = 1,  IngredientId = 61, Amount = 2,    Unit = "Stück", Note = "verquirlt" },
                new RecipeIngredient { Id = 4,   RecipeId = 1,  IngredientId = 36, Amount = 150,  Unit = "g" },
                new RecipeIngredient { Id = 5,   RecipeId = 1,  IngredientId = 23, Amount = 150,  Unit = "g",     Note = "zum Ausbacken" },
                new RecipeIngredient { Id = 6,   RecipeId = 1,  IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                new RecipeIngredient { Id = 7,   RecipeId = 1,  IngredientId = 78, Amount = 1,    Unit = "Stück" },
                // Rindsgulasch (Recipe 2)
                new RecipeIngredient { Id = 8,   RecipeId = 2,  IngredientId = 85, Amount = 800,  Unit = "g",     Note = "in Würfel geschnitten" },
                new RecipeIngredient { Id = 9,   RecipeId = 2,  IngredientId = 62, Amount = 3,    Unit = "Stück", Note = "fein gehackt" },
                new RecipeIngredient { Id = 10,  RecipeId = 2,  IngredientId = 3,  Amount = 3,    Unit = "TL" },
                new RecipeIngredient { Id = 11,  RecipeId = 2,  IngredientId = 4,  Amount = 1,    Unit = "TL" },
                new RecipeIngredient { Id = 12,  RecipeId = 2,  IngredientId = 47, Amount = 2,    Unit = "EL" },
                new RecipeIngredient { Id = 13,  RecipeId = 2,  IngredientId = 92, Amount = 200,  Unit = "ml" },
                new RecipeIngredient { Id = 14,  RecipeId = 2,  IngredientId = 90, Amount = 500,  Unit = "ml" },
                new RecipeIngredient { Id = 15,  RecipeId = 2,  IngredientId = 23, Amount = 2,    Unit = "EL" },
                new RecipeIngredient { Id = 16,  RecipeId = 2,  IngredientId = 5,  Amount = 1,    Unit = "TL" },
                new RecipeIngredient { Id = 17,  RecipeId = 2,  IngredientId = 12, Amount = 2,    Unit = "Stück" },
                new RecipeIngredient { Id = 18,  RecipeId = 2,  IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                // Wiener Tafelspitz (Recipe 3)
                new RecipeIngredient { Id = 19,  RecipeId = 3,  IngredientId = 93, Amount = 1000, Unit = "g" },
                new RecipeIngredient { Id = 20,  RecipeId = 3,  IngredientId = 64, Amount = 2,    Unit = "Stück" },
                new RecipeIngredient { Id = 21,  RecipeId = 3,  IngredientId = 72, Amount = 1,    Unit = "Stück" },
                new RecipeIngredient { Id = 22,  RecipeId = 3,  IngredientId = 62, Amount = 1,    Unit = "Stück", Note = "halbiert, angeröstet" },
                new RecipeIngredient { Id = 23,  RecipeId = 3,  IngredientId = 71, Amount = 1,    Unit = "Stück" },
                new RecipeIngredient { Id = 24,  RecipeId = 3,  IngredientId = 12, Amount = 2,    Unit = "Stück" },
                new RecipeIngredient { Id = 25,  RecipeId = 3,  IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                new RecipeIngredient { Id = 26,  RecipeId = 3,  IngredientId = 94, Amount = 4,    Unit = "EL",    Note = "zum Servieren" },
                // Backhendl (Recipe 4)
                new RecipeIngredient { Id = 27,  RecipeId = 4,  IngredientId = 81, Amount = 800,  Unit = "g",     Note = "in Stücke geteilt" },
                new RecipeIngredient { Id = 28,  RecipeId = 4,  IngredientId = 25, Amount = 100,  Unit = "g" },
                new RecipeIngredient { Id = 29,  RecipeId = 4,  IngredientId = 61, Amount = 2,    Unit = "Stück", Note = "verquirlt" },
                new RecipeIngredient { Id = 30,  RecipeId = 4,  IngredientId = 36, Amount = 200,  Unit = "g" },
                new RecipeIngredient { Id = 31,  RecipeId = 4,  IngredientId = 20, Amount = 300,  Unit = "ml",    Note = "zum Frittieren" },
                new RecipeIngredient { Id = 32,  RecipeId = 4,  IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                new RecipeIngredient { Id = 33,  RecipeId = 4,  IngredientId = 78, Amount = 1,    Unit = "Stück" },
                // Zwiebelrostbraten (Recipe 5)
                new RecipeIngredient { Id = 34,  RecipeId = 5,  IngredientId = 85, Amount = 600,  Unit = "g",     Note = "in Scheiben geschnitten" },
                new RecipeIngredient { Id = 35,  RecipeId = 5,  IngredientId = 62, Amount = 4,    Unit = "Stück", Note = "in Ringe geschnitten" },
                new RecipeIngredient { Id = 36,  RecipeId = 5,  IngredientId = 20, Amount = 3,    Unit = "EL" },
                new RecipeIngredient { Id = 37,  RecipeId = 5,  IngredientId = 21, Amount = 20,   Unit = "g" },
                new RecipeIngredient { Id = 38,  RecipeId = 5,  IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                // Kaiserschmarrn (Recipe 6)
                new RecipeIngredient { Id = 39,  RecipeId = 6,  IngredientId = 61, Amount = 4,    Unit = "Stück", Note = "getrennt" },
                new RecipeIngredient { Id = 40,  RecipeId = 6,  IngredientId = 53, Amount = 200,  Unit = "ml" },
                new RecipeIngredient { Id = 41,  RecipeId = 6,  IngredientId = 25, Amount = 100,  Unit = "g" },
                new RecipeIngredient { Id = 42,  RecipeId = 6,  IngredientId = 27, Amount = 40,   Unit = "g" },
                new RecipeIngredient { Id = 43,  RecipeId = 6,  IngredientId = 21, Amount = 30,   Unit = "g" },
                new RecipeIngredient { Id = 44,  RecipeId = 6,  IngredientId = 29, Amount = 20,   Unit = "g",    Note = "zum Bestreuen" },
                new RecipeIngredient { Id = 45,  RecipeId = 6,  IngredientId = 33, Amount = 1,    Unit = "TL" },
                // Apfelstrudel (Recipe 7)
                new RecipeIngredient { Id = 46,  RecipeId = 7,  IngredientId = 95, Amount = 1,    Unit = "Stück" },
                new RecipeIngredient { Id = 47,  RecipeId = 7,  IngredientId = 79, Amount = 6,    Unit = "Stück", Note = "geschält, entkernt" },
                new RecipeIngredient { Id = 48,  RecipeId = 7,  IngredientId = 27, Amount = 80,   Unit = "g" },
                new RecipeIngredient { Id = 49,  RecipeId = 7,  IngredientId = 7,  Amount = 2,    Unit = "TL" },
                new RecipeIngredient { Id = 50,  RecipeId = 7,  IngredientId = 36, Amount = 50,   Unit = "g",    Note = "geröstet" },
                new RecipeIngredient { Id = 51,  RecipeId = 7,  IngredientId = 21, Amount = 60,   Unit = "g",    Note = "flüssig" },
                new RecipeIngredient { Id = 52,  RecipeId = 7,  IngredientId = 29, Amount = 20,   Unit = "g",    Note = "zum Bestreuen" },
                // Semmelknödel (Recipe 8)
                new RecipeIngredient { Id = 53,  RecipeId = 8,  IngredientId = 96, Amount = 6,    Unit = "Stück", Note = "vom Vortag" },
                new RecipeIngredient { Id = 54,  RecipeId = 8,  IngredientId = 53, Amount = 200,  Unit = "ml",    Note = "warm" },
                new RecipeIngredient { Id = 55,  RecipeId = 8,  IngredientId = 61, Amount = 2,    Unit = "Stück" },
                new RecipeIngredient { Id = 56,  RecipeId = 8,  IngredientId = 21, Amount = 20,   Unit = "g" },
                new RecipeIngredient { Id = 57,  RecipeId = 8,  IngredientId = 62, Amount = 1,    Unit = "Stück", Note = "fein gewürfelt" },
                new RecipeIngredient { Id = 58,  RecipeId = 8,  IngredientId = 76, Amount = 1,    Unit = "Bund",  Note = "fein gehackt" },
                new RecipeIngredient { Id = 59,  RecipeId = 8,  IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                new RecipeIngredient { Id = 60,  RecipeId = 8,  IngredientId = 6,  Amount = 1,    Unit = "Prise" },
                // Palatschinken (Recipe 9)
                new RecipeIngredient { Id = 61,  RecipeId = 9,  IngredientId = 25, Amount = 150,  Unit = "g" },
                new RecipeIngredient { Id = 62,  RecipeId = 9,  IngredientId = 61, Amount = 2,    Unit = "Stück" },
                new RecipeIngredient { Id = 63,  RecipeId = 9,  IngredientId = 53, Amount = 300,  Unit = "ml" },
                new RecipeIngredient { Id = 64,  RecipeId = 9,  IngredientId = 21, Amount = 20,   Unit = "g",    Note = "zum Backen" },
                new RecipeIngredient { Id = 65,  RecipeId = 9,  IngredientId = 27, Amount = 10,   Unit = "g" },
                new RecipeIngredient { Id = 66,  RecipeId = 9,  IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                // Krautfleckerl (Recipe 10)
                new RecipeIngredient { Id = 67,  RecipeId = 10, IngredientId = 97, Amount = 500,  Unit = "g",    Note = "in Streifen" },
                new RecipeIngredient { Id = 68,  RecipeId = 10, IngredientId = 98, Amount = 300,  Unit = "g" },
                new RecipeIngredient { Id = 69,  RecipeId = 10, IngredientId = 62, Amount = 1,    Unit = "Stück", Note = "fein gehackt" },
                new RecipeIngredient { Id = 70,  RecipeId = 10, IngredientId = 21, Amount = 40,   Unit = "g" },
                new RecipeIngredient { Id = 71,  RecipeId = 10, IngredientId = 27, Amount = 10,   Unit = "g",    Note = "zum Karamellisieren" },
                new RecipeIngredient { Id = 72,  RecipeId = 10, IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                new RecipeIngredient { Id = 73,  RecipeId = 10, IngredientId = 5,  Amount = 1,    Unit = "TL" },
                new RecipeIngredient { Id = 74,  RecipeId = 10, IngredientId = 55, Amount = 100,  Unit = "ml" },
                // Grießnockerlsuppe (Recipe 11)
                new RecipeIngredient { Id = 75,  RecipeId = 11, IngredientId = 99, Amount = 80,   Unit = "g" },
                new RecipeIngredient { Id = 76,  RecipeId = 11, IngredientId = 21, Amount = 40,   Unit = "g",    Note = "weich" },
                new RecipeIngredient { Id = 77,  RecipeId = 11, IngredientId = 61, Amount = 1,    Unit = "Stück" },
                new RecipeIngredient { Id = 78,  RecipeId = 11, IngredientId = 6,  Amount = 1,    Unit = "Prise" },
                new RecipeIngredient { Id = 79,  RecipeId = 11, IngredientId = 90, Amount = 1500, Unit = "ml" },
                new RecipeIngredient { Id = 80,  RecipeId = 11, IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                // Linsensuppe mit Speck (Recipe 12)
                new RecipeIngredient { Id = 81,  RecipeId = 12, IngredientId = 44, Amount = 300,  Unit = "g" },
                new RecipeIngredient { Id = 82,  RecipeId = 12, IngredientId = 83, Amount = 100,  Unit = "g" },
                new RecipeIngredient { Id = 83,  RecipeId = 12, IngredientId = 62, Amount = 1,    Unit = "Stück", Note = "fein gehackt" },
                new RecipeIngredient { Id = 84,  RecipeId = 12, IngredientId = 64, Amount = 1,    Unit = "Stück", Note = "gewürfelt" },
                new RecipeIngredient { Id = 85,  RecipeId = 12, IngredientId = 72, Amount = 1,    Unit = "Stück", Note = "gewürfelt" },
                new RecipeIngredient { Id = 86,  RecipeId = 12, IngredientId = 90, Amount = 1200, Unit = "ml" },
                new RecipeIngredient { Id = 87,  RecipeId = 12, IngredientId = 89, Amount = 1,    Unit = "EL" },
                new RecipeIngredient { Id = 88,  RecipeId = 12, IngredientId = 12, Amount = 2,    Unit = "Stück" },
                // Erdäpfelgulasch (Recipe 13)
                new RecipeIngredient { Id = 89,  RecipeId = 13, IngredientId = 65, Amount = 800,  Unit = "g",    Note = "in Würfel geschnitten" },
                new RecipeIngredient { Id = 90,  RecipeId = 13, IngredientId = 62, Amount = 2,    Unit = "Stück", Note = "fein gehackt" },
                new RecipeIngredient { Id = 91,  RecipeId = 13, IngredientId = 3,  Amount = 2,    Unit = "TL" },
                new RecipeIngredient { Id = 92,  RecipeId = 13, IngredientId = 23, Amount = 2,    Unit = "EL" },
                new RecipeIngredient { Id = 93,  RecipeId = 13, IngredientId = 47, Amount = 1,    Unit = "EL" },
                new RecipeIngredient { Id = 94,  RecipeId = 13, IngredientId = 90, Amount = 800,  Unit = "ml" },
                new RecipeIngredient { Id = 95,  RecipeId = 13, IngredientId = 5,  Amount = 1,    Unit = "TL" },
                new RecipeIngredient { Id = 96,  RecipeId = 13, IngredientId = 63, Amount = 2,    Unit = "Stück" },
                // Zwetschkenknödel (Recipe 14)
                new RecipeIngredient { Id = 97,  RecipeId = 14, IngredientId = 65, Amount = 500,  Unit = "g",    Note = "gekocht und gepresst" },
                new RecipeIngredient { Id = 98,  RecipeId = 14, IngredientId = 25, Amount = 200,  Unit = "g" },
                new RecipeIngredient { Id = 99,  RecipeId = 14, IngredientId = 61, Amount = 1,    Unit = "Stück" },
                new RecipeIngredient { Id = 100, RecipeId = 14, IngredientId = 21, Amount = 20,   Unit = "g" },
                new RecipeIngredient { Id = 101, RecipeId = 14, IngredientId = 100,Amount = 12,   Unit = "Stück", Note = "entkernt" },
                new RecipeIngredient { Id = 102, RecipeId = 14, IngredientId = 29, Amount = 20,   Unit = "g",    Note = "zum Bestreuen" },
                new RecipeIngredient { Id = 103, RecipeId = 14, IngredientId = 7,  Amount = 1,    Unit = "TL" },
                new RecipeIngredient { Id = 104, RecipeId = 14, IngredientId = 36, Amount = 60,   Unit = "g",    Note = "in Butter geröstet" },
                // Faschierter Braten (Recipe 15)
                new RecipeIngredient { Id = 105, RecipeId = 15, IngredientId = 82, Amount = 800,  Unit = "g" },
                new RecipeIngredient { Id = 106, RecipeId = 15, IngredientId = 61, Amount = 2,    Unit = "Stück" },
                new RecipeIngredient { Id = 107, RecipeId = 15, IngredientId = 96, Amount = 2,    Unit = "Stück", Note = "eingeweicht und ausgedrückt" },
                new RecipeIngredient { Id = 108, RecipeId = 15, IngredientId = 62, Amount = 1,    Unit = "Stück", Note = "fein gehackt" },
                new RecipeIngredient { Id = 109, RecipeId = 15, IngredientId = 76, Amount = 1,    Unit = "Bund",  Note = "fein gehackt" },
                new RecipeIngredient { Id = 110, RecipeId = 15, IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                new RecipeIngredient { Id = 111, RecipeId = 15, IngredientId = 87, Amount = 1,    Unit = "TL" },
                new RecipeIngredient { Id = 112, RecipeId = 15, IngredientId = 83, Amount = 100,  Unit = "g",    Note = "als Belag" },
                // Gebackener Blumenkohl (Recipe 16)
                new RecipeIngredient { Id = 113, RecipeId = 16, IngredientId = 75, Amount = 1,    Unit = "Stück" },
                new RecipeIngredient { Id = 114, RecipeId = 16, IngredientId = 25, Amount = 80,   Unit = "g" },
                new RecipeIngredient { Id = 115, RecipeId = 16, IngredientId = 61, Amount = 2,    Unit = "Stück", Note = "verquirlt" },
                new RecipeIngredient { Id = 116, RecipeId = 16, IngredientId = 36, Amount = 120,  Unit = "g" },
                new RecipeIngredient { Id = 117, RecipeId = 16, IngredientId = 20, Amount = 300,  Unit = "ml",    Note = "zum Frittieren" },
                new RecipeIngredient { Id = 118, RecipeId = 16, IngredientId = 78, Amount = 1,    Unit = "Stück" },
                // Penne mit Tomatensauce (Recipe 17)
                new RecipeIngredient { Id = 119, RecipeId = 17, IngredientId = 38, Amount = 400,  Unit = "g" },
                new RecipeIngredient { Id = 120, RecipeId = 17, IngredientId = 46, Amount = 2,    Unit = "Dose" },
                new RecipeIngredient { Id = 121, RecipeId = 17, IngredientId = 62, Amount = 1,    Unit = "Stück", Note = "fein gehackt" },
                new RecipeIngredient { Id = 122, RecipeId = 17, IngredientId = 63, Amount = 2,    Unit = "Stück", Note = "gepresst" },
                new RecipeIngredient { Id = 123, RecipeId = 17, IngredientId = 19, Amount = 3,    Unit = "EL" },
                new RecipeIngredient { Id = 124, RecipeId = 17, IngredientId = 27, Amount = 5,    Unit = "g" },
                new RecipeIngredient { Id = 125, RecipeId = 17, IngredientId = 11, Amount = 1,    Unit = "TL" },
                new RecipeIngredient { Id = 126, RecipeId = 17, IngredientId = 10, Amount = 1,    Unit = "TL" },
                new RecipeIngredient { Id = 127, RecipeId = 17, IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                // Spinatknödel (Recipe 18)
                new RecipeIngredient { Id = 128, RecipeId = 18, IngredientId = 73, Amount = 300,  Unit = "g",    Note = "blanchiert und ausgedrückt" },
                new RecipeIngredient { Id = 129, RecipeId = 18, IngredientId = 96, Amount = 4,    Unit = "Stück", Note = "eingeweicht" },
                new RecipeIngredient { Id = 130, RecipeId = 18, IngredientId = 61, Amount = 2,    Unit = "Stück" },
                new RecipeIngredient { Id = 131, RecipeId = 18, IngredientId = 62, Amount = 1,    Unit = "Stück", Note = "fein gehackt" },
                new RecipeIngredient { Id = 132, RecipeId = 18, IngredientId = 21, Amount = 30,   Unit = "g" },
                new RecipeIngredient { Id = 133, RecipeId = 18, IngredientId = 60, Amount = 60,   Unit = "g",    Note = "frisch gerieben" },
                new RecipeIngredient { Id = 134, RecipeId = 18, IngredientId = 25, Amount = 50,   Unit = "g" },
                new RecipeIngredient { Id = 135, RecipeId = 18, IngredientId = 6,  Amount = 1,    Unit = "Prise" },
                new RecipeIngredient { Id = 136, RecipeId = 18, IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                // Leberknödelsuppe (Recipe 19)
                new RecipeIngredient { Id = 137, RecipeId = 19, IngredientId = 101,Amount = 300,  Unit = "g",    Note = "fein gehackt" },
                new RecipeIngredient { Id = 138, RecipeId = 19, IngredientId = 96, Amount = 2,    Unit = "Stück", Note = "eingeweicht" },
                new RecipeIngredient { Id = 139, RecipeId = 19, IngredientId = 61, Amount = 1,    Unit = "Stück" },
                new RecipeIngredient { Id = 140, RecipeId = 19, IngredientId = 62, Amount = 1,    Unit = "Stück", Note = "fein gehackt" },
                new RecipeIngredient { Id = 141, RecipeId = 19, IngredientId = 76, Amount = 1,    Unit = "Bund",  Note = "fein gehackt" },
                new RecipeIngredient { Id = 142, RecipeId = 19, IngredientId = 90, Amount = 1500, Unit = "ml" },
                new RecipeIngredient { Id = 143, RecipeId = 19, IngredientId = 1,  Amount = 1,    Unit = "Prise" },
                new RecipeIngredient { Id = 144, RecipeId = 19, IngredientId = 6,  Amount = 1,    Unit = "Prise" },
                // Topfenstrudel (Recipe 20)
                new RecipeIngredient { Id = 145, RecipeId = 20, IngredientId = 95, Amount = 1,    Unit = "Stück" },
                new RecipeIngredient { Id = 146, RecipeId = 20, IngredientId = 58, Amount = 500,  Unit = "g" },
                new RecipeIngredient { Id = 147, RecipeId = 20, IngredientId = 61, Amount = 2,    Unit = "Stück", Note = "getrennt" },
                new RecipeIngredient { Id = 148, RecipeId = 20, IngredientId = 27, Amount = 80,   Unit = "g" },
                new RecipeIngredient { Id = 149, RecipeId = 20, IngredientId = 33, Amount = 1,    Unit = "TL" },
                new RecipeIngredient { Id = 150, RecipeId = 20, IngredientId = 78, Amount = 1,    Unit = "Stück", Note = "Schale gerieben" },
                new RecipeIngredient { Id = 151, RecipeId = 20, IngredientId = 29, Amount = 20,   Unit = "g",    Note = "zum Bestreuen" }
            );

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
