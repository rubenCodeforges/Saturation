using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnter : MonoBehaviour
{
    public ParticleSystem triggerParticles;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spawnParticles(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spawnParticles(other);
        }
    }
    
    private void spawnParticles(Collider2D other)
    {
        if (!other.GetComponent<CharacterControl>().getIsGrounded())
        {
            ParticleSystem instance = Instantiate(triggerParticles);
            float positionBottom = other.transform.position.y - other.GetComponent<CircleCollider2D>().radius / 2;
            instance.transform.position = new Vector3(other.transform.position.x, positionBottom, other.transform.position.z);
        }
    }
}