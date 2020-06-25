public class KeypressEvent : Event
{
    public readonly InputManager.UsefulKeys key;


    public KeypressEvent(InputManager.UsefulKeys key)
    {
        this.key = key;
    }
}
