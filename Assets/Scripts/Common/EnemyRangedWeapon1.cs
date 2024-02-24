using System.Collections;
using UnityEngine;

public class EnemyRangedWeapon: Weapon2D
{
    [SerializeField] private Transform projectileSpawnPoint; // Single spawn point for projectiles
    [SerializeField] private GameObject projectilePrefab; // Reference to your projectile prefab
    [SerializeField] private float projectileSpeed = 10f;

    public override void Attack(eDirection direction)
    {
        
    }

    public override bool Use(Animator animator)
    {
        bool used = false;
        if (ready)
        {
            if (animator != null && animationTriggerName != "")
            {
                animator.SetTrigger(animationTriggerName);
                ready = false;
                StartCoroutine(ResetAttackReadyCR(attackRate));
                Attack();

                used = true;
            }
        }
        return used;
    }

    private void Attack()
    {
        // Assuming the player is tagged with "Player"
        GameObject player = GameObject.FindWithTag("Player");
        if (projectilePrefab && projectileSpawnPoint && player != null)
        {
            // Calculate direction towards the player
            Vector2 direction = (player.transform.position - projectileSpawnPoint.position).normalized;

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            // Launch the projectile towards the player
            if (projectileScript != null)
            {
                projectileScript.Launch(direction * projectileSpeed);
            }
        }
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
            // Assuming the player is tagged with "Player"
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                // Draw a line from the spawn point to the player
                Gizmos.color = Color.red; // Set the color of the Gizmo line
                Gizmos.DrawLine(projectileSpawnPoint.position, player.transform.position);

                // Optionally, draw a sphere at the spawn point to visualize it
                Gizmos.DrawWireSphere(projectileSpawnPoint.position, 0.5f);

                // Draw a sphere at the player's position to visualize the target
                Gizmos.DrawWireSphere(player.transform.position, 0.5f);
            }

        }
    }

}
