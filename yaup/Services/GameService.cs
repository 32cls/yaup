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

    public async Task StartGame(string roomName, IHubCallerClients clients)
    {
        Games.TryGetValue(roomName, out Game game);
        if (game == null || game.Players.Count != 4)
        {
            throw new Exception();
        }
        else
        {
            game.RoomName = roomName;
            game.Clients = clients;
            while (game.BlueTeamScore < 1000 || game.RedTeamScore < 1000)
            {
                var round = new Round(game);
                game.Rounds.Add(round);
                await round.Start();
            }
        }
    }

    public async Task EvaluateCard(string roomName, bool picked, Colors? trumpColor, string user, IHubCallerClients clients)
    {
        Games.TryGetValue(roomName, out Game game);
        if (game == null)
        {
            throw new Exception();
        }
        var currentRound = game.Rounds.Last();
        if (currentRound.CurrentPlayer.Id != user)
        {
            throw new Exception();
        }
        await currentRound.PlayerAnswer(picked, trumpColor);        
    }
}