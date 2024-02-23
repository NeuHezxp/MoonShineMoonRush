using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Character2D : MonoBehaviour
{
	public enum eFace
	{
		Left,
		Right
	}

	[SerializeField] protected SpriteRenderer spriteRenderer;
	[SerializeField] protected Animator animator;

	[SerializeField, Range(0, 10)] protected float speed = 5;
	[SerializeField, Range(0, 80)] protected float gravity = 60;
	[SerializeField] protected float health = 100;
	[SerializeField] protected eFace spriteFacing = eFace.Right;

	protected CharacterController2D characterController;

	protected Vector2 movement = Vector2.zero;
	protected eFace facing = eFace.Right;


	protected virtual void Start()
	{
		characterController = GetComponent<CharacterController2D>();
	}
    protected virtual void Update()
    {
        // Assuming horizontal input is used to determine the direction
        float horizontalInput = Input.GetAxis("Horizontal");

        // Update the facing direction based on the input
        if (horizontalInput > 0)
        {
            facing = eFace.Right;
        }
        else if (horizontalInput < 0)
        {
            facing = eFace.Left;
        }
    }

   



    protected virtual void FixedUpdate()
	{
		// vertical movement (gravity)
		movement.y -= gravity * Time.fixedDeltaTime;
		movement.y = Mathf.Max(movement.y, -gravity * Time.fixedDeltaTime * 3);

		characterController.Move(movement * Time.fixedDeltaTime);
		UpdateFacing();
	}

    protected void UpdateFacing()
    {
        // Flip the sprite based on the facing direction
        spriteRenderer.flipX = (facing != spriteFacing);
    }

}
