using UnityEngine;

public class CharacterBase : MonoBehaviour, Damageable
{
    [Header("Character Stats")]
    [SerializeField] protected float health = 100f;
    protected float currentHealth;

    [Header("Attack Settings")]
    [SerializeField] protected float attackRate = 1f; // Attack cooldown in seconds
    protected float attackCooldown = 0f;
    [SerializeField] protected int attackDamage = 10; // Damage per attack
    [SerializeField] protected float range = 2f; // Attack range in world units
    [SerializeField] protected GameObject attackPrefab; // Attack object (e.g., projectile)

    [Header("Components")]
    [SerializeField] protected Transform rayOrigin; // Where attacks originate
    [SerializeField] protected Vector3 rayDirection = Vector3.right; // Default attack direction
    [SerializeField] protected LayerMask targetLayer; // Layer to detect attackable objects

    [Header("Character Settings")]
    [SerializeField] protected Animator animator;
    protected virtual void Start()
    {
        currentHealth = health; // Initialize health
        if(rayOrigin == null)
        {
            rayOrigin = transform;
        }
    }

    protected virtual void Update()
    {
        // Handle attack cooldown
        if (attackRate > 0 && attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        // Perform attack if cooldown is ready
        if (attackCooldown <= 0)
        {
            DetectAndAttack();
        }
    }

    // Detects enemies and attacks when in range
    protected virtual void DetectAndAttack()
    {
        float rayDistance = range * 2; // Convert range to world distance

        // Perform a raycast to detect an enemy
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, rayDirection, rayDistance, targetLayer);

        if (hit.collider != null)
        {
            Attack(hit.collider.gameObject);
        }
    }

    // Handles attack logic
    protected virtual void Attack(GameObject target)
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // If using projectiles
        if (attackPrefab != null)
        {
            Instantiate(attackPrefab, rayOrigin.position, Quaternion.identity);
        }

        // Direct damage if melee
        else if (target.TryGetComponent(out Damageable enemy))
        {
            enemy.TakeDamage(attackDamage);
        }

        attackCooldown = attackRate; // Reset cooldown
    }

    // Handles taking damage
    public virtual void TakeDamage(int damage) // Marked as virtual
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Death();
    }

    protected virtual void Death() // Also virtual if GnomeBase or others need to override it
    {
        // Default death logic (can be empty or generalized)
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (rayOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(rayOrigin.position, rayOrigin.position + (rayDirection * (range * 2)));
        }
    }
}
