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
            game.Deck = new Deck();
            game.Deck.Cards = [.. game.Deck.Cards.OrderBy(_ => rng.Next())];
            game.Deck.Display();
            game.RoomName = roomName;
            game.Clients = clients;
            var round = new Round(game);
            game.Rounds.Add(round);
            await round.Start();
        }
    }

    public async Task EvaluateCard(string roomName, bool picked, Colors? trumpColor, string userId, IHubCallerClients clients)
    {
        Games.TryGetValue(roomName, out Game game);
        if (game == null)
        {
            throw new Exception();
        }
        var currentRound = game.Rounds.Last();
        if (currentRound.Game.Players.ElementAt(currentRound.CurrentIndex).Id != userId)
        {
            throw new Exception();
        }
        await currentRound.PlayerAnswer(picked, trumpColor);        
    }

    public async Task PlayCard(string roomName, string userId, Card card)
    {
        Games.TryGetValue(roomName, out Game game);
        if (game == null)
        {
            throw new Exception();
        }
        var currentTrick = game.Rounds.Last().Tricks.Last();
        await currentTrick.PlayCard(card, userId);
    }
}