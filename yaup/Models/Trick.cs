public class Trick(Player master, Colors trumpColor)
{
    public Player Master = master;

    public Colors TrumpColor = trumpColor;

    public Card FirstCard;

    public void Start()
    {

    }

    public async Task PlayCard(Card card)
    {
        if (FirstCard == null)
        {
            FirstCard = card;
        }
        else
        {
            CheckCard(card);
        }
    }

    private bool CheckCard(Card card)
    {
        if (card.Color == FirstCard.Color)
        {
            return true;
        }
        else
        {
            if (card.Color == TrumpColor)
            {

            }
        }
        return false;
        
    }
}