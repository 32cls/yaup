using Microsoft.AspNetCore.SignalR;

public interface IGameService
{
    public Dictionary<string, Game> Games { get; }

    public void JoinOrCreateGame(string roomName, Player player);

    public Task EvaluateCard(string roomName, bool picked, Colors? trumpColor, string userId, IHubCallerClients clients);

    public Task StartGame(string roomName, IHubCallerClients clients);
    
}