using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 6f;
    [SerializeField] float climbSpeed = 6f;
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myBoxCollider;
    float gravityScaleAtStart;

    bool isAlive = true;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = playerRigidbody.gravityScale;
        
    }


    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; } 
        
        moveInput = value.Get<Vector2>();
        
        
        //Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){return;}
        

        if (value.isPressed) 
        {
            //jump activated
            playerRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value) 
    {
        if (!isAlive) { return; }
        if (value.isPressed)
        {
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        
    }

    //flip player art if moving
    void FlipSprite()
    {
        
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed) 
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }
        
    }

    //climb ladder 
    void ClimbLadder()
    {
        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            playerRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;

        }

        Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x, moveInput.y * climbSpeed);
        playerRigidbody.velocity = climbVelocity;
        playerRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(playerRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    //die when touching enemy
    void Die()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            playerRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    

}
