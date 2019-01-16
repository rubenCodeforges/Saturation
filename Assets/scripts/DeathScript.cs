using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public ParticleSystem deathParticles;
    public Rigidbody2D playerRb;
    public GameObject accelerationEffect;
    
    private void OnTriggerEnter2D(Collider2D collider2D)
    {            
        
        if (collider2D.tag == "Damage")
        {
            KillPlayer();
            FindObjectOfType<GameManager>().endGame();

        }
        else if (collider2D.tag == "Acceleration")
        {
            accelerationEffect.transform.position = playerRb.position;
            Instantiate(accelerationEffect);            
        }
        else if (collider2D.tag == "Exit")
        {
            gameObject.SetActive(false);
            FindObjectOfType<GameManager>().endLevel();
        }
    }

    private void KillPlayer()
    {
        deathParticles.transform.position = playerRb.position;
        Instantiate(deathParticles);
        gameObject.SetActive(false);
    }
}
