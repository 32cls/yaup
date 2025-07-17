class GameController
{
    private static readonly Random rng = new();
    public void CreateNewGame(List<Player> players)
    {
        var deck = new Deck();
        deck.Cards = [.. deck.Cards.OrderBy(_ => rng.Next())];
        DrawInitialCards(deck, players);
        deck.Display();
    }

    void DrawInitialCards(Deck deck, List<Player> players)
    {
        foreach (var player in players)
        {
            player.Hand.AddRange(deck.Cards.GetRange(0, 3));
            deck.Cards.RemoveRange(0, 3);
        }
        foreach (var player in players)
        {
            player.Hand.AddRange(deck.Cards.GetRange(0, 2));
            deck.Cards.RemoveRange(0, 2);
            player.DisplayHand();
        }
    }
}