using System.Collections;
using UnityEngine;
using static Weapon2D;

// Inherit from Character2D and interfaces for damage, healing, and scoring
public class Player2D : Character2D, IDamagable, IHealable, IScoreable
{
    [Header("Movement Settings")]
    [SerializeField, Range(0, 20)] private float jumpHeight = 12;
    private int jumpCount = 0;
    [SerializeField] private int maxJumpCount = 2; // Allow for double jump

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private float cooldownTimer = 0f;
    [SerializeField] private int maxDashCount = 1;
    private int currentDashCount = 0;

    [Header("Player Stats")]
    [SerializeField] private IntVariable scoreVar;
    [SerializeField] private FloatVariable healthVar;

    [Header("Weapons")]
    [SerializeField] private Weapon2D weaponMelee;
    [SerializeField] public Weapon2D rangedWeapon;

    private void Update()
    {
        HandleMovementInput();
        HandleJump();
        HandleDash();
        HandleAttackInput();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); // Process movement and physics
    }

    // Handle horizontal movement based on player input
    private void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        movement.x = horizontalInput * speed;

        // Update animator with movement speed
        animator.SetFloat("Speed", Mathf.Abs(movement.x));

        // Update facing direction based on the input
        if (Mathf.Abs(movement.x) > 0.1f)
        {
            facing = (movement.x > 0) ? eFace.Right : eFace.Left;
        }
    }


    // Jumping logic
    private void HandleJump()
    {
        bool isGrounded = characterController.onGround;
        animator.SetBool("OnGround", isGrounded);

        if (isGrounded)
        {
            jumpCount = 0; // Reset jump count
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || jumpCount < maxJumpCount)
            {
                movement.y = jumpHeight;
                jumpCount++;

                // Check if it's a double jump
                if (jumpCount == 1) 
                {
                    animator.SetTrigger("DoubleJump");
                }
            }
        }
    }

    // Dash logic with cooldown and ground check
    private void HandleDash()
    {
        bool isGrounded = characterController.onGround;
        if (Input.GetKeyDown(KeyCode.LeftShift) && cooldownTimer <= 0 && !isDashing && isGrounded && currentDashCount < maxDashCount)
        {
            StartCoroutine(PerformDash());
            currentDashCount++;
        }

        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }
    private IEnumerator PerformDash()
    {
        isDashing = true;
        animator.SetTrigger("Dash");
        float originalSpeed = speed; // Save original speed
        speed += dashSpeed; // Increase speed for momentum
        cooldownTimer = dashCooldown; // Start cooldown

        Vector2 dashDirection = new Vector2(facing == eFace.Right ? 1 : -1, 0) * dashSpeed;
        characterController.Move(dashDirection * Time.deltaTime);

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            yield return null; // Maintain momentum for the duration of the dash
        }

        speed = originalSpeed; // Reset speed after dash
        isDashing = false;
    }

    // Handle player attacks
    private void HandleAttackInput()
    {
        //if (Input.GetButtonDown("Fire1")) rangedWeapon.Use(animator);
        //if (Input.GetButtonDown("Fire2")) weaponMelee.Use(animator); 
        if (Input.GetButtonDown("Fire1"))
        {
            rangedWeapon.Use(animator); 
            animator.SetTrigger("Shoot"); 
        }
        if (Input.GetButtonDown("Fire2"))
        {
            weaponMelee.Use(animator);
            animator.SetTrigger("Attack");
        }
    }

    public void TriggerMeleeAttack()
    {
        
        if (weaponMelee != null)
        {
            eDirection direction = facing == eFace.Right ? eDirection.Right : eDirection.Left;
            weaponMelee.Attack(direction);
        }
    }

    // Implementing IDamagable, IHealable, IScoreable interfaces
    public void ApplyDamage(float damage)
    {
        healthVar.value -= damage;
        Debug.Log("Player Damaged: " + damage);
        if (healthVar.value <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player Died");

        //gameObject.SetActive(false);

        animator.SetTrigger("Death");
    }

    public void Heal(float health)
    {
        healthVar.value += health;
        Debug.Log("Player Healed: " + health);
    }

    public void AddScore(int score)
    {
        scoreVar.value += score;
        Debug.Log("Score Updated: " + score);
        UIManager.Instance.Score = scoreVar.value;
    }
}
