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
    // public float gravity;   

    public LayerMask groundMask;
    private bool isGrounded;
    public Transform feetPosition;
    public float groundCheckCircle;

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
        if (input < 0){
            playerSprite.flipX = true;
        } else if (input > 0){
            playerSprite.flipX = false;
        }

        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundMask);

        if (isGrounded == true && Input.GetButton("Jump")){
            playerRb.velocity = Vector2.up * jumpForce;
        }


    }

    void FixedUpdate()
    {
        playerRb.velocity = new Vector2(input * speed, playerRb.velocity.y);
    }
}
