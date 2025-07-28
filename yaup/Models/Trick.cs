using System.Drawing;
using Microsoft.AspNetCore.SignalR;

public class Trick(Player master, Colors trumpColor, int playerIndex, Game game)
{
    public Player Master = master;

    public Colors TrumpColor = trumpColor;

    public int PlayerIndex = playerIndex;

    public Card[] PlayedCards = [];

    public Game Game = game;

    public void Start()
    {

    }

    public async Task PlayCard(Card card, string userId)
    {
        var hand = Game.Players.ElementAt(PlayerIndex).Hand;
        if (userId != Game.Players.ElementAt(PlayerIndex).Id || !hand.Contains(card))
        {
            throw new Exception();
        }
        if (PlayedCards.First() == null)
        {
            PlayedCards.Append(card);
        }
        else
        {
            if (CheckCard(card))
            {
                PlayedCards.Append(card);
            }
        }
        hand.Remove(card);
        await Game.Clients.User(userId).SendAsync("HandUpdate", hand);
        PlayerIndex = (PlayerIndex + 1) % 4;
        if (PlayedCards.Length == 4)
        {
            UpdateMaster();
        }
    }

    private void UpdateMaster()
    {
        int max = 0;
        int maxIndex = 0;
        for (int index = 0; index < 4; index++)
        {
            int value = ComputeCardValue(PlayedCards.ElementAt(index));
            if (value > max)
            {
                max = value;
                maxIndex = index;
            }
        }
        Master = Game.Players.ElementAt(maxIndex);        
    }

    private bool CheckCard(Card card)
    {
        if (card.Color == PlayedCards.First().Color)
        {
            return true;
        }
        else
        {
            if (!HasFirstColorInHand() && card.Color == TrumpColor)
            {
                if (PlayedCards.Length == 1)
                {
                    return true;
                }
                if (ComputeCardValue(card) > ComputeCardValue(PlayedCards.Last()))
                {
                    return true;
                }
                if (!HasGreaterTrumpInHandThan(PlayedCards.Last()))
                {
                    return true;
                }
            }
            else if (!HasFirstColorInHand() && !HasTrumpInHand())
            {
                return true;
            }
            else if (!HasFirstColorInHand() && HasTrumpInHand() && Master != Game.Players.ElementAt((PlayerIndex + 2) % 4) && card.Color == TrumpColor)
            {
                return true;
            }
        }
        return false;

    }

    private bool HasFirstColorInHand() {
        return Game.Players.ElementAt(PlayerIndex).Hand.Find(card => card.Color == PlayedCards.First().Color) != null;
    }

    private bool HasTrumpInHand()
    {
        return Game.Players.ElementAt(PlayerIndex).Hand.Find(card => card.Color == TrumpColor) != null;
    }

    private bool HasGreaterTrumpInHandThan(Card givenCard)
    {
        return Game.Players.ElementAt(PlayerIndex).Hand.Find(card => card.Color == TrumpColor && ComputeCardValue(card) > ComputeCardValue(givenCard)) != null;
    }

    private int ComputeCardValue(Card card) {
        return card.Rank switch
        {
            "7" or "8" => 0,
            "9" => card.Color == TrumpColor ? 14 : 0,
            "10" => 10,
            "A" => 11,
            "J" => card.Color == TrumpColor ? 20 : 2,
            "Q" => 3,
            "K" => 4,
            _ => throw new Exception(),
        };
    }
}