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



    /// <summary>
    /// Functions
    /// </summary>


    //In Setup(), assign the contactParticle
    public virtual void Setup()
    {
        rb = GetComponent<Rigidbody>();
        swordGlow = transform.Find(GLOW_OBJ).GetComponent<Light>();
        CurrentIntensity = SwordManager.SwordIntensity.Normal;
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
}
