using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemyMovement = 1f;
    Rigidbody2D enemyRigidbody;
    BoxCollider2D enemyBoxCollider;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyBoxCollider = GetComponent<BoxCollider2D>();
    }

 
    void Update()
    {
        
        enemyRigidbody.velocity = new Vector2(enemyMovement, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        enemyMovement = -enemyMovement;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), 1f);
    }
}
