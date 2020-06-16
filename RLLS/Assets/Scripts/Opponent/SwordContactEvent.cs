using UnityEngine;

public class SwordContactEvent : Event
{
    /// <summary>
    /// Fields
    /// </summary>


    public readonly Rigidbody rb; //the sword sending out the event
    public readonly Collision collision;


    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="rb">The rigidbody associated with the sword sending out the event.</param>
    /// <param name="collision">Collision data, with information about the other sword.</param>


    public SwordContactEvent(Rigidbody rb, Collision collision)
    {
        this.rb = rb;
        this.collision = collision;
    }
}
