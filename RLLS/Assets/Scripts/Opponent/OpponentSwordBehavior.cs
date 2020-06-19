using UnityEngine;

public class OpponentSwordBehavior : SwordBehavior
{
    private const string OPPONENT_PARTICLE_NAME = "P2 sword particle";


    /// <summary>
    /// Assign the player sword's contact particle system
    /// </summary>
    public override void Setup()
    {
        base.Setup();
        contactParticle = GameObject.Find(OPPONENT_PARTICLE_NAME).GetComponent<ParticleSystem>();
    }
}
