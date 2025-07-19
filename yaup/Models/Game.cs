using Microsoft.AspNetCore.SignalR;

public class Game
{
    public SortedSet<Player> Players = [];
    public Deck Deck = new Deck();
    public List<Round> Rounds = [];

    public int RedTeamScore;

    public int BlueTeamScore;

    public IHubCallerClients Clients;

    public string RoomName;

    public Game()
    {

    }
}