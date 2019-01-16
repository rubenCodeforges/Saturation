using UnityEngine;

public class MassPowerUpPickUP : MonoBehaviour
{
    public float massFactorMultiplier = 1.5f;
    public float coreInitialScale = 0.1f;
    
    public ParticleSystem[] ParticleSystems;
    public GameObject player;
    public GameObject massCenter;
    public ParticleSystem pickupEffect;

    public float massCorePickupAnimationSpeed = 1f;
    
    [HideInInspector]
    public bool isPickedUp;
    
    private readonly int playerMassCoreIndex = 0;
    private Vector3 massCoreScale;
    
    private void Start()
    {
        init();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody.tag == "Player" && !isPickedUp)
        {
            foreach (ParticleSystem system in ParticleSystems)
            {
                system.transform.position = player.transform.position;
                system.Stop();
                pickupEffect.gameObject.SetActive(true);
            }

            isPickedUp = true;
            increasePlayersMass();
        }
    }

    private void Update()
    {
        if (isPickedUp)
        {
            bindPowerUpToPlayer();
        }
    }

    private void increasePlayersMass()
    {
        massCenter.SetActive(false);
        Rigidbody2D rb = player.GetComponentInParent<Rigidbody2D>();
        rb.mass = rb.mass * massFactorMultiplier;
    }

    private void init()
    {
        massCoreScale = new Vector3(
            coreInitialScale * massFactorMultiplier,
            coreInitialScale * massFactorMultiplier,
            coreInitialScale * massFactorMultiplier
        );
        massCenter.transform.localScale = massCoreScale;
    }

    private void bindPowerUpToPlayer()
    {
        
        foreach (ParticleSystem system in ParticleSystems)
        {
            system.transform.position = player.transform.position;
        }

        increasePlayersMassCore();
    }
    
    private void increasePlayersMassCore()
    {
        var playerMassCore = player.transform.GetChild(playerMassCoreIndex);
        Vector3 targetScale = massCoreScale * massFactorMultiplier;
        
        if (playerMassCore.localScale != targetScale)
        {
            playerMassCore.localScale = Vector3.Lerp(playerMassCore.localScale, targetScale, massCorePickupAnimationSpeed);            
        }
    }

}
