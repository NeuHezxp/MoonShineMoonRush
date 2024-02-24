using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f; // Damage dealt by the projectile
    [SerializeField] private float speed = 10f; // Speed of the projectile
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction)
    {
        Debug.Log($" Enemy Launching projectile in direction: {direction}");
        rb.velocity = direction.normalized * speed;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object hit is not tagged as Enemy
        if (!collision.CompareTag("Enemy"))
        {
            // Check if the object hit implements the IDamagable interface
            IDamagable hitObject = collision.GetComponent<IDamagable>();
            if (hitObject != null)
            {
                hitObject.ApplyDamage(damage);
            }

            // Destroy the projectile in any case after hitting
            Destroy(gameObject);
        }
    }

}
