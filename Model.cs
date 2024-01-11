using Microsoft.EntityFrameworkCore;

public class DataBaseContext : DbContext
{
    // Terminal commands
    // dotnet tool install --global dotnet-ef
    // dotnet add package Microsoft.EntityFrameworkCore.Design
    // dotnet ef migrations add InitialCreate
    // dotnet ef database update

    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }

    public string DbPath { get; }

    public DataBaseContext()
    {
        var folder = Environment.SpecialFolder.Desktop;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "teams.db");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Team>().Property(t => t.Name).IsRequired();
        modelBuilder.Entity<Team>().Property(t => t.Country).IsRequired(false);

        // This 1:N relationship is created by convention (adding the fields Players, TeamId and Team)
        // However its also possible to define manually.
        modelBuilder.Entity<Team>()
            .HasMany(t => t.Players)
            .WithOne(p => p.Team)
            .HasForeignKey(p => p.TeamId)
            .IsRequired();

        modelBuilder.Entity<Player>().Property(p => p.Name).IsRequired();
        modelBuilder.Entity<Player>().Property(p => p.Position).IsRequired(false);
    }
}

public class Team
{
    // Primary key
    public int Id { get; set; }

    // Mandatory field
    public string Name { get; set; } = null!;

    // Optional field
    public string? Country { get; set; }

    // Collection Navigation
    public List<Player> Players { get; } = new List<Player>();
}

public class Player
{
    public int Id { get; set; }

    // Mandatory field
    public string Name { get; set; } = null!;

    // Optional field
    public string? Position { get; set; }

    // Required foreign key
    public int TeamId { get; set; }

    // Required reference navigation to principal
    public Team Team { get; set; } = null!;
}