public class CardinalDirectionEvent : Event
{
    public readonly KeyboardInput.CardinalDirections cardinalDir;


    public CardinalDirectionEvent(KeyboardInput.CardinalDirections cardinalDir)
    {
        this.cardinalDir = cardinalDir;
    }
}
