using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private AudioSource sfx;

    private Collider2D collider;
    private float journeyLength, distCovered, fracJourney;
    private float startTime, speed = 1;
    private bool coinCollected = false;
    private Transform collectionPoint;
    private float multiplier = 10.5f;

    private void OnTriggerEnter2D(Collider2D other) //on collision with this coin prefab the value of the coin is added to coins collected in the gamemanager
    {
        if(other.tag == "Player")
        {
            startTime = Time.time;
            collider.enabled = false;
            coinCollected = true;
            SoundHandler.singleton.PlayCoinSound();
            Manager.singleton.AddScore(value * multiplier);
        }
    }

    void Start()
    {
        collider = gameObject.GetComponent<CircleCollider2D>();
        collectionPoint = Manager.singleton.coinCollectPosition;
    }

    void Update()
    {
        // Start moving only when coin is collected
        if (!coinCollected) return;

        journeyLength = Vector2.Distance(transform.position, collectionPoint.position);
        distCovered = (Time.time - startTime) * 2.5f;
        fracJourney = ((distCovered) / journeyLength);
        transform.position = Vector3.Lerp(transform.position, collectionPoint.position, fracJourney);

        // When coin reaches end position - stop update and handle coin destroy
        if (Vector2.Distance(transform.position, collectionPoint.position) < 0.5f)
        {
            Manager.singleton.AddCoins(value);
            Destroy(gameObject);
        }
    }
}
