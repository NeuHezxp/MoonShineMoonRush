using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] public Transform raycast;
    [SerializeField] public LayerMask raycastMask;

    public float raycastLength;
    public float attackMinDistance;
    public float moveSpeed;
    public float timer;

    private RaycastHit2D hit;
    private GameObject target;
    private Animator animator;
    private float distance;
    private float intTimer;

    private bool attack;
    private bool inRange;
    private bool cooldown;

    private void Awake()
    {
        intTimer = timer;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (inRange) 
        {
            hit = Physics2D.Raycast(raycast.position, Vector2.left, raycastLength, raycastMask);
            RaycastDebugger();
        }

        // when player is detected
        if (hit.collider != null)
        { 
            EnemyLogic();
        }
        else if (hit.collider == null) 
        { 
            inRange = false;
        }

        if (inRange == false)
        {
            animator.SetBool("Walking", false);
            StopAttacking();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        { 
            target = collision.gameObject;
            inRange = true;
        }
    }

    private void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > attackMinDistance)
        {
            Move();
            StopAttacking();
        }
        else if (attackMinDistance > distance) 
        {
            Attack();
        }

        if (cooldown)
        {
            animator.SetBool("Attack", false);
        }
    }

    private void Move()
    {
        animator.SetBool("Walking", true);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack")) // change attack to the sprite name
        { 
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        timer = intTimer; // resets timer when player enters range
        attack = true; // checks if enemy can attack

        animator.SetBool("Walking", false);
        animator.SetBool("Attacking", true);
    }

    private void StopAttacking()
    {
        cooldown = false;
        attack = false;
        animator.SetBool("Attacking", false);
    }

    private void RaycastDebugger()
    {
        if (distance > attackMinDistance)
        {
            Debug.DrawRay(raycast.position, Vector2.left * raycastLength, Color.red);
        }
        else if (attackMinDistance > distance)
        { 
            Debug.DrawRay(raycast.position, Vector2.left * raycastLength, Color.green);
        }
    }
}
