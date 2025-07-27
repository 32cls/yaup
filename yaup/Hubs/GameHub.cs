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

        public async Task JoinRoom(string roomName)
        {
            var player = new Player(Context.UserIdentifier, Context.User.Identity.Name);
            GameService.JoinOrCreateGame(roomName, player);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", roomName, Context.User.Identity.Name);
        }

        public async Task StartGame(string roomName)
        {
            await GameService.StartGame(roomName, Clients);
        }

        public async Task EvaluateCard(string roomName, bool picked, Colors? trumpColor)
        {
            await GameService.EvaluateCard(roomName, picked, trumpColor, Context.UserIdentifier, Clients);
        }

        public async Task PlayCard(string roomName, Card card)
        {
            await GameService.PlayCard(roomName, Context.UserIdentifier, card);
        }
    }
}