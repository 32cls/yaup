using System.Drawing;

public class Trick(Player master, Colors trumpColor, int playerIndex, HashSet<Player> players)
{
    public Player Master = master;

    public Colors TrumpColor = trumpColor;

    public int PlayerIndex = playerIndex;

    public HashSet<Player> Players = players;

    public Card[] PlayedCards = [];

    public void Start()
    {

    }

    public async Task PlayCard(Card card, string userId)
    {
        if (userId != Players.ElementAt(PlayerIndex).Id || !Players.ElementAt(PlayerIndex).Hand.Contains(card))
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
        Players.ElementAt(PlayerIndex).Hand.Remove(card);
        PlayerIndex = (PlayerIndex + 1) % 4;
    }

    private bool CheckCard(Card card)
    {
        if (PlayedCards.First() == null) {
            return true;
        }
        else {
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
                else if (!HasFirstColorInHand() && HasTrumpInHand() && Master != Players.ElementAt((PlayerIndex + 2) % 4) && card.Color == TrumpColor)
                {
                    return true;
                }
            }
        }
        return false;

    }

    private bool HasFirstColorInHand() {
        return Players.ElementAt(PlayerIndex).Hand.Find(card => card.Color == PlayedCards.First().Color) != null;
    }

    private bool HasTrumpInHand()
    {
        return Players.ElementAt(PlayerIndex).Hand.Find(card => card.Color == TrumpColor) != null;
    }

    private bool HasGreaterTrumpInHandThan(Card givenCard)
    {
        return Players.ElementAt(PlayerIndex).Hand.Find(card => card.Color == TrumpColor && ComputeCardValue(card) > ComputeCardValue(givenCard)) != null;
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