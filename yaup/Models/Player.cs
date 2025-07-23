using Microsoft.EntityFrameworkCore;

public class Player(string id, string name)
{
    public string ConnectionId { get; set; } = id;
    public string Name { get; set; } = name;
    public List<Card> Hand { get; } = [];

    public void DisplayHand()
    {
        Console.WriteLine("{0}'s hand:", Name);
        foreach (var card in Hand)
        {
            Console.WriteLine("{0} of {1}", card.Rank, card.Color);
        }
    }
}

class PlayerDb(DbContextOptions options) : DbContext(options)
{
    public DbSet<Player> Players { get; set; } = null!;
}