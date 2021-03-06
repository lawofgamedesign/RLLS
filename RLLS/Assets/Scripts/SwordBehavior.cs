﻿using UnityEngine;

public abstract class SwordBehavior : MonoBehaviour
{

    /// <summary>
    /// Fields
    /// </summary>
    protected ParticleSystem contactParticle; //each sword should specify its own particle system for particle to play when making contact
    protected Rigidbody rb;



    /// <summary>
    /// Functions
    /// </summary>


    //In Setup(), assign the contactParticle
    public virtual void Setup()
    {
        rb = GetComponent<Rigidbody>();
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
}
