class Player
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<Card> Hand { get; } = [];

    public Player(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public void DisplayHand()
    {
        Console.WriteLine("{0}'s hand:", Name);
        foreach (var card in Hand)
        {
            Console.WriteLine("{0} of {1}", card.Rank, card.Color);
        }
    }
}