using UnityEngine;

public class PlayerSwordBehavior : SwordBehavior
{
    private const string PLAYER_PARTICLE_NAME = "P1 sword particle";


    private GameObject attackPlane;
    private const string ATTACK_READY_PLANE = "Player attack plane";
    private const string SCENE_OBJ = "Scene";


    /// <summary>
    /// Assign the player sword's contact particle system
    /// </summary>
    public override void Setup()
    {
        base.Setup();
        contactParticle = GameObject.Find(PLAYER_PARTICLE_NAME).GetComponent<ParticleSystem>();
        attackPlane = GameObject.Find(SCENE_OBJ).transform.Find(ATTACK_READY_PLANE).gameObject; //find the attack plane even if it's currently shut off
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == attackPlane)
        {
            Services.Swords.ChangeIntensity(SwordManager.Swords.Player, SwordManager.SwordIntensity.Full);
            Services.Swordfighters.SetVulnerability(Services.Swordfighters.Opponent, true);
            Services.Events.Fire(new WindupEvent()); //used by the training sequence to detect that the player has figured out how to wind up
        }
    }



}
