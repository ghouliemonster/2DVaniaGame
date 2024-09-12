using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSound;
    [SerializeField] int coinPickupAmount = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(coinPickupAmount);
            AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
