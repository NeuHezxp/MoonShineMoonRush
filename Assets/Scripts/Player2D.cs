using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : Character2D, IDamagable,IHealable,IScoreable
{
	//game mangager has these and the player has the 
	[SerializeField] IntVariable scoreVar;
	[SerializeField] FloatVariable healthVar;
	[SerializeField] Weapon2D weapon;

	[SerializeField, Range(0, 20)] float jump = 12;


    // Jumping logic
    private void HandleJump()
    {
        bool isGrounded = characterController.onGround;
        animator.SetBool("OnGround", isGrounded);


		if (Input.GetButtonDown("Fire1"))
		{
			weapon.Use(animator);
		}
	}

	protected override void FixedUpdate()
	{
		// horizontal movement
		movement.x = Input.GetAxis("Horizontal") * speed;
		animator.SetFloat("Speed", Mathf.Abs(movement.x));
		if (Mathf.Abs(movement.x) > 0.1f) facing = (movement.x > 0) ? eFace.Right : eFace.Left;

		base.FixedUpdate();
	}

	public void Attack()
	{
		Weapon2D.eDirection direction = (facing == eFace.Right) ? Weapon2D.eDirection.Right : Weapon2D.eDirection.Left;
		weapon.Attack(direction);
	}


        Vector2 dashDirection = new Vector2(facing == eFace.Right ? 1 : -1, 0) * dashSpeed;
        characterController.Move(dashDirection * Time.deltaTime);


	public void Heal(float health)
	{
		healthVar.value += health;
        print("Player Healed:" + health);
	}

    public void AddScore(int score)
    {
       scoreVar.value += score;//this
    }
}
