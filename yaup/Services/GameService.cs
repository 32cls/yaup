using Microsoft.AspNetCore.SignalR;

public class GameService: IGameService
{
    private static readonly Random rng = new();

    public Dictionary<string, Game> Games { get; } = [];

    public void JoinOrCreateGame(string roomName, Player player)
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

    private async Task DrawInitialCards(Game game, IHubCallerClients clients)
    {
        foreach (var player in game.Players)
        {
            player.Hand.AddRange(game.Deck.Cards.GetRange(0, 3));
            await clients.User(player.Id).SendAsync("HandUpdate", player.Hand);
            game.Deck.Cards.RemoveRange(0, 3);
        }
        foreach (var player in game.Players)
        {
            player.Hand.AddRange(game.Deck.Cards.GetRange(0, 2));
            await clients.User(player.Id).SendAsync("HandUpdate", player.Hand);
            game.Deck.Cards.RemoveRange(0, 2);
            player.DisplayHand();
        }
    }

    public async Task StartGame(string roomName, IHubCallerClients clients)
    {
        Games.TryGetValue(roomName, out Game game);
        if (game == null || game.Players.Count != 4)
        {
            throw new Exception();
        }
        else {
            await DrawInitialCards(game, clients);
        }        
    }

}