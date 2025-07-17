interface IGameController
{
    public Dictionary<string, Game> Games { get; }

    void JoinOrCreateGame(string roomName, Player player);

    void StartGame(string roomName);
    
}