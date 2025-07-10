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

    private bool jumpRequested = false;

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");

        if (input < 0){
            playerSprite.flipX = true;
        } else if (input > 0){
            playerSprite.flipX = false;
        }

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)) && !jumpRequested)
        {
            StartCoroutine(DelayedJump());
        }
    }

    IEnumerator DelayedJump()
    {
        jumpRequested = true;
        yield return new WaitForSeconds(0.05f); // jump delay

        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundMask);

        if (isGrounded)
        {
            playerRb.velocity = Vector2.up * jumpForce;
        }

        jumpRequested = false; // reset for next jump
    }

    void FixedUpdate()
    {
        playerRb.velocity = new Vector2(input * speed, playerRb.velocity.y);
    }
}
