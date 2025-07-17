using Microsoft.AspNetCore.SignalR;

namespace yaup.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            var gameController = new GameController();
            Player[] players = [new Player(1, "Arno"), new Player(2, "Ayoub"), new Player(3, "Jonasz"), new Player(4, "Belle")];
            gameController.CreateNewGame([.. players]);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}