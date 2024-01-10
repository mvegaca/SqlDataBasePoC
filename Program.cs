using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            using (var db = new DataBaseContext())
            {
                await db.Database.MigrateAsync();
                await db.Database.EnsureCreatedAsync();

                var isInitializingData = true;
                if (isInitializingData)
                {
                    var bcn = new Team()
                    {
                        Name = "FC Barcelona",
                        Country = "Spain",
                    };
                    var pbcn1 = new Player()
                    {
                        Name = "Marc Andre Ter Stegen",
                        Position = "GK",
                    };
                    bcn.Players.Add(pbcn1);
                    await db.AddAsync(bcn);
                    await db.SaveChangesAsync();

                    var rm = new Team()
                    {
                        Name = "Real Madrid CF",
                        Country = "Spain",
                    };
                    await db.AddAsync(rm);
                    await db.SaveChangesAsync();
                }
                else
                {
                    foreach (var team in db.Teams.Include(t => t.Players))
                    {
                        System.Diagnostics.Debug.WriteLine($"Team - Id: {team.Id} Name: {team.Name} Country: {team.Country} Players total: {team.Players.Count}");
                        foreach (var player in team.Players)
                        {
                            System.Diagnostics.Debug.WriteLine($"Player - Id: {player.Id} Name: {player.Name} Position: {player.Position} TeamId: {player.TeamId} Team: {player.Team.Name}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }

        Console.ReadLine();
    }
}