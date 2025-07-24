using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace yaup.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        public readonly IGameService GameService;

        public GameHub(IGameService gameService)
        {
            GameService = gameService;
        }

        public async Task JoinRoom(string roomName, string userName)
        {
            var player = new Player(Context.UserIdentifier, userName);
            GameService.JoinOrCreateGame(roomName, player);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", roomName, userName);
        }

        public async Task StartGame(string roomName)
        {
            await GameService.StartGame(roomName, Clients);
        }

        public async Task EvaluateCard(string userName, string roomName, bool picked, Colors? trumpColor)
        {
            await GameService.EvaluateCard(roomName, picked, trumpColor, userName, Clients);
        }
    }
}