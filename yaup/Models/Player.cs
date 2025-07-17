class Player(string? id, string name)
{
    public string? Id { get; set; } = id;
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