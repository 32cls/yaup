using Microsoft.AspNetCore.SignalR;

public class Round
{
    private static readonly Random rng = new();
    public int StarterIndex;

    public Player CurrentPlayer;

    public List<Trick> Tricks = [];

    public Game Game;

    public bool RevealedCardPicked = false;

    public int TurnCounter;

    public Colors TrumpColor;

    public Round(Game game)
    {
        Game = game;
        StarterIndex = rng.Next(0, 4);
        CurrentPlayer = Game.Players.ElementAt(StarterIndex);
        TurnCounter = 0;
    }

    public async Task Start()
    {
        while (!RevealedCardPicked)
        {
            await DrawInitialCards();
            await PickOrPass();
        }

    }

    private async Task DrawInitialCards()
    {
        foreach (var player in Game.Players)
        {
            player.Hand.AddRange(Game.Deck.Cards.GetRange(0, 3));
            await Game.Clients.User(player.Id).SendAsync("HandUpdate", player.Hand);
            Game.Deck.Cards.RemoveRange(0, 3);
        }
        foreach (var player in Game.Players)
        {
            player.Hand.AddRange(Game.Deck.Cards.GetRange(0, 2));
            await Game.Clients.User(player.Id).SendAsync("HandUpdate", player.Hand);
            Game.Deck.Cards.RemoveRange(0, 2);
            player.DisplayHand();
        }
        await Game.Clients.Group(Game.RoomName).SendAsync("RevealedCard", Game.Deck.Cards.First());
    }

    public async Task PickOrPass()
    {
        await Game.Clients.User(CurrentPlayer.Id).SendAsync("PickOrPass");
    }

    public async Task PlayerAnswer(bool picked, Colors? trumpColor)
    {
        if (CurrentPlayer.Id == Game.Players.ElementAt(StarterIndex).Id)
        {
            TurnCounter++;
            if (TurnCounter == 3)
            {
                return;
            }
        }
        if (picked)
        {
            var topCard = Game.Deck.Cards.First();
            if (TurnCounter == 1)
            {
                TrumpColor = topCard.Color;
                CurrentPlayer.Hand.Add(topCard);
            }
            else if (TurnCounter == 2)
            {
                if (trumpColor != null && trumpColor.Value != Game.Deck.Cards.First().Color)
                {
                    TrumpColor = trumpColor.Value;
                    CurrentPlayer.Hand.Add(topCard);
                    await DrawRemainingCards();
                    StartTricks();
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        else
        {
            CurrentPlayer = Game.Players.ElementAt(StarterIndex + 1 % 4);
            await PickOrPass();
        }
    }

    private async Task DrawRemainingCards()
    {
        foreach (var player in Game.Players)
        {
            if (player.Id == CurrentPlayer.Id)
            {
                player.Hand.AddRange(Game.Deck.Cards.GetRange(0, 2));
                Game.Deck.Cards.RemoveRange(0, 2);
            }
            else
            {
                player.Hand.AddRange(Game.Deck.Cards.GetRange(0, 3));
                Game.Deck.Cards.RemoveRange(0, 3);
            }
            player.DisplayHand();            
            await Game.Clients.User(player.Id).SendAsync("HandUpdate", player.Hand);
        }
    }

    private void StartTricks()
    {
        for (var i = 0; i < 8; i++)
        {
            var trick = new Trick();
            trick.Start();
            Tricks.Add(trick);
        }
    }



}