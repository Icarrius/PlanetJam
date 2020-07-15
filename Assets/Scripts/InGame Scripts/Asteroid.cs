using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject particleEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            CreateParticleAnimation();
            Destroy(gameObject);
        }
        else if(other.tag != "Boundary")
        {
            Destroy(gameObject);
        }
    }

    private void CreateParticleAnimation()
    {
        Destroy(Instantiate(particleEffect, transform.position, Quaternion.identity), 2);
    }
}
