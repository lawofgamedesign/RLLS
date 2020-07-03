using UnityEngine;

public class PlayerSwordBehavior : SwordBehavior
{
    private const string PLAYER_PARTICLE_NAME = "P1 sword particle";


    private GameObject attackPlane;
    private const string ATTACK_READY_PLANE = "Player attack plane";


    /// <summary>
    /// Assign the player sword's contact particle system
    /// </summary>
    public override void Setup()
    {
        base.Setup();
        contactParticle = GameObject.Find(PLAYER_PARTICLE_NAME).GetComponent<ParticleSystem>();
        attackPlane = GameObject.Find(ATTACK_READY_PLANE);
    }
}
