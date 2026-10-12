using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int pointValue = 1;
    public float lifetime = 5f;

    private void Start()
    {
        // Automatically destroy the coin after 5 seconds if not collected
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the coin
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.AddScore(pointValue);
            Destroy(gameObject);
        }
    }
}