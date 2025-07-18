using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class GameService : IGameService
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

    private async Task DrawInitialCards(Game game, IHubCallerClients clients, string roomName)
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
        await clients.Group(roomName).SendAsync("RevealedCard", game.Deck.Cards.First());
    }

    private async Task PickOrPass(Game game, IHubCallerClients clients)
    {
        await clients.User(game.Players.ElementAt(game.Starter).Id).SendAsync("PickOrPass");
    }

    public async Task EvaluateCard(string roomName, bool picked, string user, IHubCallerClients clients)
    {
        Games.TryGetValue(roomName, out Game game);
        if (game == null || user != game.CurrentPlayer.Name)
        {
            throw new Exception();
        }
        if (picked)
        {
            var revealedCard = game.Deck.Cards.First();
            game.CurrentPlayer.Hand.Add(revealedCard);
            game.Deck.Cards.RemoveAt(0);
        }
        else
        {
            NextPlayer(game);
            await PickOrPass(game, clients);
        }
    }

    public async Task StartGame(string roomName, IHubCallerClients clients)
    {
        Games.TryGetValue(roomName, out Game game);
        if (game == null || game.Players.Count != 4)
        {
            throw new Exception();
        }
        else
        {
            await DrawInitialCards(game, clients, roomName);
            game.Starter = rng.Next(0, 5);
            await PickOrPass(game, clients);
        }
    }

    private void NextPlayer(Game game)
    {
        game.Starter = (game.Starter + 1) % 4; 
    }

}