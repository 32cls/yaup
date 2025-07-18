using System.Collections.Immutable;

public enum Colors
{
    Spades,
    Hearts,
    Diamonds,
    Clubs
}

public class Card(Colors Color, string Rank)
{
    public Colors Color { get; } = Color;
    public string Rank { get; set; } = Rank;

}

public class Deck
{
    private static readonly ImmutableArray<string> _ranks = ["7", "8", "9", "10", "J", "Q", "K", "A"];

    public List<Card> Cards { get; set; } = [];

    public Deck()
    {
        var colors = Enum.GetValues<Colors>();
        foreach (var rank in _ranks)
        {
            foreach (var color in colors)
            {
                Cards.Add(new Card(color, rank));
            }
        }

    }

    public void Display()
    {
        Console.WriteLine("Cards in deck:");
        foreach (var card in Cards)
        {
            Console.WriteLine("{0} of {1}", card.Rank, card.Color);
        }
    }
}