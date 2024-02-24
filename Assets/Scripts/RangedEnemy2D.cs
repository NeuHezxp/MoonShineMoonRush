using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy2D : Character2D,IDamagable
{
	enum eState
	{
		Idle,
		Patrol,
		Chase,
		Attack,
		Death
	}

	[SerializeField] AIPerception2D perception;
	[SerializeField] AIPath2D path2D;
    

    [SerializeField] private GameObject projectilePrefab; // Reference to your projectile prefab
    [SerializeField] private Transform projectileSpawnPoint; // Spawn point for projectiles
    [SerializeField] private float projectileSpeed = 10f; // Speed of the projectile


    [SerializeField] float attackRange = 2;
	[SerializeField] float maxHealth = 2;
	[SerializeField] Weapon2D Weapon2D;

	private eState state;
	private float timer;

    private float attackCooldown = 1f; // Cooldown in seconds between attacks
    private float lastAttackTime = -1f; // Track the last attack time


    private GameObject enemy = null;


	protected override void Start()
	{
		base.Start();

		health = maxHealth;
		state = eState.Idle;
		timer = 2;
	}
		
	void Update()
	{
		var sensed = perception.GetSensedGameObjects(); //tries to perceive objects as enemies
		enemy = (sensed.Length > 0) ? sensed[0] : null; //enemy not null then chase

			switch (state)
		{
			case eState.Idle:
				timer -= Time.deltaTime;
				if (timer <= 0)
				{
					state = eState.Patrol;
				}
				break;
			case eState.Patrol:
				if (enemy != null)
				{
					state = eState.Chase;
				}
				break;
            case eState.Chase:
                if (enemy == null)
                {
                    state = eState.Patrol;
                }
                else if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange && Time.time > lastAttackTime + attackCooldown)
                {
                    state = eState.Attack;
                }
                break;
            case eState.Attack:
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    LaunchProjectileAtPlayer();
                    Attack();
                    lastAttackTime = Time.time; // Update the last attack time
                                                // Do not immediately switch back to chase; wait for attack to be acknowledged in FixedUpdate
                }
                break;

            case eState.Death:
				animator.SetBool("Death", true);
				movement.x = 0;
                if (!isCoroutineStarted) // Check if the coroutine has already been started to avoid duplicates
                {
                    StartCoroutine(DestroyAfterDelay(2f)); // Destroy the enemy after 2 seconds
                    isCoroutineStarted = true; // Ensure we don't start multiple coroutines
                }
                break;
		}


	}

	protected override void FixedUpdate()
	{
		// horizontal movement
		if (state == eState.Patrol)
		{
			movement.x = (transform.position.x < path2D.targetPosition.x) ? speed : -speed; //if position is less then target position then the enemy moves to a point
		}
		if (state == eState.Chase)
		{
			movement.x = (transform.position.x < enemy.transform.position.x) ? speed : -speed;
			if (Mathf.Abs(transform.position.x - enemy.transform.position.x) < attackRange)
			{
				state = eState.Attack;
				Weapon2D.Use(animator);

				animator.SetTrigger("Attack");
			}

		}
		if(state == eState.Attack)
		{
            movement.x = 0;
            if (Time.time > lastAttackTime)            {
                state = eState.Chase; 
            }

        }
        if (state == eState.Death)
		{
			movement.x = 0;
		}
		animator.SetFloat("Speed", Mathf.Abs(movement.x));
		if (Mathf.Abs(movement.x) > 0.1f) facing = (movement.x > 0) ? eFace.Right : eFace.Left;


		base.FixedUpdate();
	}
	public void AttackDone()
	{
		if (state != eState.Death)
		{
			state = eState.Chase;
		}

		state = eState.Chase;	
    }

    public void Attack()
    {
        //rangedWeapon.Use(animator);
    }

    public void ApplyDamage(float damage)
    {
        health -=damage;
		if(health <= 0)
		{
            state = eState.Death;
        }
    }
    private void LaunchProjectileAtPlayer()
    {
        if (enemy != null && projectilePrefab && projectileSpawnPoint)
        {
            Vector2 direction = (enemy.transform.position - projectileSpawnPoint.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
        }
    }

    private bool isCoroutineStarted = false; // To ensure the coroutine isn't started multiple times

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject); // Destroys the enemy game object
    }

    void OnDrawGizmos()
    {
        // Use a distinctive color to make the attack range easily visible
        Gizmos.color = Color.yellow;

        // Draw a wire sphere to represent the attack range
        if (attackRange > 0)
        {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
