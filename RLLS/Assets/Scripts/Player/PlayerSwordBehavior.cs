using UnityEngine;

public class PlayerSwordBehavior : SwordBehavior
{
    private const string PLAYER_PARTICLE_NAME = "P1 sword particle";


    /// <summary>
    /// Assign the player sword's contact particle system
    /// </summary>
    public override void Setup()
    {
        base.Setup();
        contactParticle = GameObject.Find(PLAYER_PARTICLE_NAME).GetComponent<ParticleSystem>();
    }
}
