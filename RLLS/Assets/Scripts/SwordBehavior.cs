using UnityEngine;

public abstract class SwordBehavior : MonoBehaviour
{

    /// <summary>
    /// Fields
    /// </summary>
    protected ParticleSystem contactParticle; //each sword should specify its own particle system for particle to play when making contact
    protected Rigidbody rb;


    //sword lighting & particles
    protected Light swordGlow;
    protected const string GLOW_OBJ = "Sword glow";
    protected const float NORMAL_INTENSITY = 0.5f;
    protected const float FULL_INTENSITY = 5.0f;

    public SwordManager.SwordIntensity CurrentIntensity { get; private set; }


    //glowing blade edge
    protected Transform edgePartices;
    protected const string EDGE_OBJ = "Edge particles";
    protected const float FORWARD_Z_POS = 0.66f;
    protected const float BACKWARD_Z_POS = -1.0f;
    protected Vector3 edgeForwardLocalPos = new Vector3(0.0f, -1.0f, FORWARD_Z_POS);
    protected Vector3 edgeBackwardLocalPos = new Vector3(0.0f, -1.0f, BACKWARD_Z_POS);



    /// <summary>
    /// Functions
    /// </summary>


    //In Setup(), assign the contactParticle
    public virtual void Setup()
    {
        rb = GetComponent<Rigidbody>();
        swordGlow = transform.Find(GLOW_OBJ).GetComponent<Light>();
        CurrentIntensity = SwordManager.SwordIntensity.Normal;
        edgePartices = transform.Find(EDGE_OBJ);
    }


    protected virtual void OnCollisionEnter(Collision collision)
    {
        Services.Events.Fire(new SwordContactEvent(rb, collision));
    }


    protected virtual void OnCollisionStay(Collision collision)
    {
        FireContactParticle(collision.GetContact(0).point);
    }


    protected virtual void FireContactParticle(Vector3 contactPoint)
    {
        contactParticle.transform.SetPositionAndRotation(contactPoint, Quaternion.identity);
        contactParticle.Play();
    }


    public void ChangeIntensity(SwordManager.SwordIntensity newIntensity)
    {
        CurrentIntensity = newIntensity;

        swordGlow.intensity = CurrentIntensity == SwordManager.SwordIntensity.Normal ? NORMAL_INTENSITY : FULL_INTENSITY;
    }


    public void ChangeEdgePos(SwordManager.BladePositions newPos)
    {
        edgePartices.localPosition = newPos == SwordManager.BladePositions.Forward ? edgeForwardLocalPos : edgeBackwardLocalPos;
    }
}
