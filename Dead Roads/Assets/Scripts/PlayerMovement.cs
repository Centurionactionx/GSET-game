using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public float speed;
    public float input;
    public SpriteRenderer playerSprite;
    public float jumpForce;

    public LayerMask groundMask;
    private bool isGrounded;
    public Transform feetPosition;
    public float groundCheckCircle;

    public Animator animator;
    // public Sprite defaultSprite;
    // public Sprite jumpFlashSprite;

    private bool jumpRequested = false;

    void Start(){
        animator = GetComponent<Animator>();
    }

    void Update()
{
    input = Input.GetAxisRaw("Horizontal");

    if (input < 0){
        playerSprite.flipX = true;
    } else if (input > 0){
        playerSprite.flipX = false;
    }

    // Constantly check grounded status
    isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundMask);

    // Update animator with jump state
    animator.SetBool("isJumping", !isGrounded);

    if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && !jumpRequested)
    {
        // StartCoroutine(PlayJumpFlash()); // plays sprite briefly
        StartCoroutine(DelayedJump());
    }


    if (transform.position.y < -30f)
    {
        transform.position = new Vector3(0.621f, 0.038f, 0f);
        playerRb.velocity = Vector2.zero;
    }
    }

    // IEnumerator PlayJumpFlash()
    // {
    //     playerSprite.sprite = jumpFlashSprite;
    //     yield return new WaitForSeconds(0.1f); // duration to show jump flash
    //     playerSprite.sprite = defaultSprite;
    // }

    IEnumerator DelayedJump()
    {
        jumpRequested = true;
        yield return new WaitForSeconds(0.05f); // jump delay

        if (isGrounded)
        {
            playerRb.velocity = Vector2.up * jumpForce;
        }

        jumpRequested = false; // reset for next jump
    }

    void FixedUpdate()
    {
        playerRb.velocity = new Vector2(input * speed, playerRb.velocity.y);
        animator.SetFloat("x_velocity", Mathf.Abs(playerRb.velocity.x));
        animator.SetFloat("y_velocity", playerRb.velocity.y);    
    }
}
