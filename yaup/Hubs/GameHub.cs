using Microsoft.AspNetCore.SignalR;

namespace yaup.Hubs
{
    public class GameHub : Hub
    {
        public readonly IGameService GameService;

        public GameHub(IGameService gameService)
        {
            GameService = gameService;
        }

        public async Task JoinRoom(string roomName, string userName)
        {
            GameService.JoinOrCreateGame(roomName, new Player(Context.UserIdentifier, userName));                
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", roomName, userName);
        }

        public async Task StartGame(string roomName)
        {
            await GameService.StartGame(roomName, Clients);
        }
    }
}