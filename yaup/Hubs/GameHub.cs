using Microsoft.AspNetCore.SignalR;

namespace yaup.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGameController _gameController;

        public GameHub(IGameController gameController)
        {
            _gameController = gameController;
        }

        public async Task JoinRoom(string roomName, string userName)
        {
            _gameController.JoinOrCreateGame(roomName, new Player(Context.UserIdentifier, userName));                
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", roomName, userName);
        }

        public async Task StartGame(string roomName)
        {
            _gameController.StartGame(roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", roomName);
        }
    }
}