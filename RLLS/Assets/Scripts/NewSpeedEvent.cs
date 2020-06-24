public class NewSpeedEvent : Event
{
    public readonly float newSpeed;



    public NewSpeedEvent(float newSpeed)
    {
        this.newSpeed = newSpeed;
    }
}
