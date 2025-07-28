using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D playerRb;
    public float speed;
    public float jumpForce;
    private float input;

    [Header("Jumping & Ground Detection")]
    public LayerMask groundMask;
    public Transform feetPosition;
    public float groundCheckRadius = 0.1f;
    private bool isGrounded;
    private bool jumpRequested = false;

    [Header("Coyote Time")]
    public float coyoteTimeDuration = 0.8f; // Duration in seconds
    private float coyoteTimeCounter;


    [Header("Animation & Sprites")]
    public SpriteRenderer playerSprite;
    public Animator animator;

    [Header("Checkpoints")]
    public Vector3 defaultStartPosition = new Vector3(0.621f, 0.038f, 0f);
    public Vector2[] checkpointPositions; // Set in Inspector
    private bool[] checkpointTriggered;
    private Vector3 lastCheckpointPosition;

    [Header("Audio")]
    public AudioClip checkpointSoundClip;
    public AudioSource checkpointAudioSource; // Assign this to a dedicated child AudioSource in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        lastCheckpointPosition = defaultStartPosition;

        // Initialize checkpoint tracking
        checkpointTriggered = new bool[checkpointPositions.Length];
    }

    void Update()
    {
        HandleInput();
        HandleDirection();

        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckRadius, groundMask);
        animator.SetBool("isJumping", !isGrounded);

        // Start/reset coyote timer
        if (isGrounded)
            coyoteTimeCounter = coyoteTimeDuration;
        else
            coyoteTimeCounter -= Time.deltaTime;


        HandleJump();
        HandleReset();
        CheckCheckpoints();
    }


    void FixedUpdate()
    {
        playerRb.velocity = new Vector2(input * speed, playerRb.velocity.y);
        animator.SetFloat("x_velocity", Mathf.Abs(playerRb.velocity.x));
        animator.SetFloat("y_velocity", playerRb.velocity.y);
    }

    void HandleInput()
    {
        input = Input.GetAxisRaw("Horizontal");
    }

    void HandleDirection()
    {
        if (input < 0)
            playerSprite.flipX = true;
        else if (input > 0)
            playerSprite.flipX = false;
    }

    void HandleGroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckRadius, groundMask);
        animator.SetBool("isJumping", !isGrounded);
    }

    void HandleJump()
    {
        if (coyoteTimeCounter > 0f && (Input.GetButton("Jump") || Input.GetKey(KeyCode.W)))
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            coyoteTimeCounter = 0f; // Prevent double jump during coyote
        }
    }




    void HandleReset()
    {
        if (transform.position.y < -200f || Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
    }

    void CheckCheckpoints()
    {
        for (int i = 0; i < checkpointPositions.Length; i++)
        {
            if (!checkpointTriggered[i] && transform.position.x >= checkpointPositions[i].x)
            {
                // Store both X and Y from the checkpoint location
                lastCheckpointPosition = new Vector3(checkpointPositions[i].x, checkpointPositions[i].y, 0f);
                checkpointTriggered[i] = true;

                if (checkpointSoundClip != null)
                {
                    checkpointAudioSource.Stop();
                    checkpointAudioSource.clip = checkpointSoundClip;
                    checkpointAudioSource.Play();
                }
            }
        }
    }

    IEnumerator DelayedJump()
    {
        jumpRequested = true;
        yield return new WaitForSeconds(0.001f);

        if (isGrounded)
        {
            playerRb.velocity = Vector2.up * jumpForce;
        }

        jumpRequested = false;
    }

    void ResetPosition()
    {
        transform.position = lastCheckpointPosition;
        playerRb.velocity = Vector2.zero;
    }
}
