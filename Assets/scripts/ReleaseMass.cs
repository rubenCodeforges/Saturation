using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseMass : MonoBehaviour
{
    public ParticleSystem massEmissionParticles;
    public float emissionForce = 1f;
    public float massConsumption = 0.1f;
    
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = massEmissionParticles.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            Rigidbody2D playerRb = gameObject.GetComponent<Rigidbody2D>();
            float minPlayerMass = gameObject.GetComponent<CharacterControl>().initialMass;
            GameObject massCore = GameObject.Find("MassCore");
            
            disablePowerUpPickUp();
            
            if (playerRb.mass > minPlayerMass)
            {
                massEmissionParticles.Emit(1);
                playerRb.velocity = new Vector2(playerRb.velocity.x, emissionForce);
                playerRb.mass -= massConsumption;
                massCore.transform.localScale = Vector3.Lerp(massCore.transform.localScale,
                    massCore.transform.localScale - massCore.transform.localScale, massConsumption);
            }
        }
    }

    private static void disablePowerUpPickUp()
    {
        GameObject.Find("MassPowerUp").GetComponent<MassPowerUpPickUP>().isPickedUp = false;
    }

    private void LateUpdate()
    {
        massEmissionParticles.transform.rotation = initialRotation;
    }
}
