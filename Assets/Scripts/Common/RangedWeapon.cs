using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangedWeapon: Weapon2D
{
    [SerializeField] private Transform projectileSpawnPoint; // Single spawn point for projectiles
    [SerializeField] private GameObject projectilePrefab; // Reference to your projectile prefab
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int maxShots = 10;

    private int shotsRemaining; // Tracks the number of shots left

    public void Start()
    {
        shotsRemaining = maxShots; 
    }

    public override void Attack(eDirection direction)
    {
        
    }

    public override bool Use(Animator animator)
    {
        bool used = false;
        if (ready && shotsRemaining > 0) // Check if there are shots remaining
        {
            if (animator != null && animationTriggerName != "")
            {
                animator.SetTrigger(animationTriggerName);
                ready = false;
                StartCoroutine(ResetAttackReadyCR(attackRate));
                Attack();
                shotsRemaining--; // Decrement the shot counter
                used = true;
            }
        }
        return used;
    }

    private void Attack()
    {
        if (projectilePrefab && projectileSpawnPoint)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0; // Ensure there is no Z-axis difference

            Vector2 direction = (mouseWorldPosition - projectileSpawnPoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.Launch(direction * projectileSpeed);
            }
        }
    }
    public void RefillAmmo(int amount)
    {
        shotsRemaining = Mathf.Min(shotsRemaining + amount, maxShots);
    }



    IEnumerator ResetAttackReadyCR(float time)
    {
        yield return new WaitForSeconds(time);
        ready = true;
    }
    void OnDrawGizmos()
    {
        if (projectileSpawnPoint != null)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0; // Align with the 2D world

            // Draw a line from the spawn point to the mouse position
            Gizmos.color = Color.red;
            Gizmos.DrawLine(projectileSpawnPoint.position, mouseWorldPosition);

            // Optionally, draw a small sphere at the mouse position to indicate the exact target
            Gizmos.DrawSphere(mouseWorldPosition, 0.1f);
        }
    }
}
