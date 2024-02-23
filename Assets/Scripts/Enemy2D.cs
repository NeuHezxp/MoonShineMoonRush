using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2D : Character2D, IDamagable
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

	[SerializeField] float attackRange = 2;
	[SerializeField] int maxHealth = 2;
	[SerializeField] Weapon2D Weapon2D;

	private eState state;
	private float timer;

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
					timer = 2;
					state = eState.Idle;
				}
				break;
			case eState.Attack:
				//Waits for attack done 
				break;
			case eState.Death:
				animator.SetBool("Death", true);
				movement.x = 0;
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
		if(state == eState.Death)
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
		Weapon2D.eDirection direction = (facing == eFace.Right) ? Weapon2D.eDirection.Right : Weapon2D.eDirection.Left;
		Weapon2D.Attack(direction);
	}

    public void ApplyDamage(int damage)
    {
        health -=damage;
		if(health <= 0)
		{
            state = eState.Death;
        }
    }
}
