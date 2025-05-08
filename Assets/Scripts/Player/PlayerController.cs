using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }
    public static PlayerController Instance;

    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private bool facingLeft = false;
    private bool canMove = true;

    private INGAMEAM audioManager;

    // Walking sound control
    private bool isWalking = false;
    private float walkSoundCooldown = 0.4f;
    private float walkSoundTimer = 0f;

    public MathCManager mm;

    private void Awake()
    {
        audioManager = FindObjectOfType<INGAMEAM>();

        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        if (!canMove) return;

        PlayerInput();
        HandleWalkingSound();

        // 🔥 Easy attack input (LEFT MOUSE BUTTON or SPACEBAR)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
            FacingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            FacingLeft = false;
        }
    }

    private void HandleWalkingSound()
    {
        if (movement.magnitude > 0.1f)
        {
            if (!isWalking)
            {
                isWalking = true;
                walkSoundTimer = walkSoundCooldown;
                audioManager.PlaySFX(audioManager.walk);
            }
            else
            {
                walkSoundTimer -= Time.deltaTime;
                if (walkSoundTimer <= 0f)
                {
                    walkSoundTimer = walkSoundCooldown;
                    audioManager.PlaySFX(audioManager.walk);
                }
            }
        }
        else
        {
            isWalking = false;
            walkSoundTimer = 0f;
        }
    }

    // 🔪 Sword attack logic
    private void Attack()
    {
        myAnimator.SetTrigger("Attack"); // Make sure your Animator has a "Attack" Trigger

        if (audioManager != null && audioManager.sword != null)
        {
            audioManager.PlaySFX(audioManager.sword);
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
        if (!enabled)
        {
            movement = Vector2.zero;
            myAnimator.SetFloat("moveX", 0);
            myAnimator.SetFloat("moveY", 0);
        }
    }

    public void Die()
    {
        SetMovementEnabled(false);
        rb.linearVelocity = Vector2.zero;
        myAnimator.SetBool("IsDead", true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collect letter M
        if (other.gameObject.CompareTag("CollectibleM"))
        {
            Destroy(other.gameObject); // Destroy the collectible object
            mm.CollectItem('M'); // Collect the "M" letter
        }

        // Collect letter A
        if (other.gameObject.CompareTag("CollectibleA"))
        {
            Destroy(other.gameObject); // Destroy the collectible object
            mm.CollectItem('A'); // Collect the "A" letter
        }

        // Collect letter T
        if (other.gameObject.CompareTag("CollectibleT"))
        {
            Destroy(other.gameObject); // Destroy the collectible object
            mm.CollectItem('T'); // Collect the "T" letter
        }

        // Collect letter H
        if (other.gameObject.CompareTag("CollectibleH"))
        {
            Destroy(other.gameObject); // Destroy the collectible object
            mm.CollectItem('H'); // Collect the "H" letter
        }
    }
}
