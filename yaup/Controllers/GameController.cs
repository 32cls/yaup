class GameController: IGameController
{
    private static readonly Random rng = new();

    public Dictionary<string, Game> Games { get; } = [];

    void IGameController.JoinOrCreateGame(string roomName, Player player)
    {
        Games.TryGetValue(roomName, out Game game);
        if (game != null)
        {
            if (game.Players.Count == 4)
            {
                throw new Exception();
            }
            else
            {
                game.Players.Add(player);
            }
        }
        else
        {
            game = new Game();
            game.Players.Add(player);
            Games.Add(roomName, game);
        }
        game.Deck.Cards = [.. game.Deck.Cards.OrderBy(_ => rng.Next())];
        game.Deck.Display();
    }

    private void DrawInitialCards(Game game)
    {
        foreach (var player in game.Players)
        {
            player.Hand.AddRange(game.Deck.Cards.GetRange(0, 3));
            game.Deck.Cards.RemoveRange(0, 3);
        }
        foreach (var player in game.Players)
        {
            player.Hand.AddRange(game.Deck.Cards.GetRange(0, 2));
            game.Deck.Cards.RemoveRange(0, 2);
            player.DisplayHand();
        }
    }

    public void StartGame(string roomName)
    {
        Games.TryGetValue(roomName, out Game game);
        if (game == null)
        {
            throw new Exception();
        }
        else {
            DrawInitialCards(game);
        }        
    }
}