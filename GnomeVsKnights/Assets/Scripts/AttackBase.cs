using UnityEngine;
using System.Collections.Generic;

public class AttackBase : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] protected LayerMask targets; // Layer of enemies to hit
    [SerializeField] protected float hitRadius = 0.5f; // Area of effect
    [SerializeField] protected float damage = 10f; // Damage dealt per hit (float)
    [SerializeField] protected float attackLifespan = 2f; // Time before attack disappears
    [SerializeField] protected int maxHits = 1; // Max enemies hit

    private int currentHits = 0;
    private HashSet<GameObject> targetsHit = new HashSet<GameObject>(); // Store unique hit targets

    protected virtual void Start()
    {
        Invoke(nameof(DestroyAttack), attackLifespan); // Auto-destroy after lifespan
    }

    protected virtual void FixedUpdate()
    {
        // Get all colliders within hit radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, hitRadius, targets);

        foreach (Collider2D hit in hits)
        {
            if (hit != null && currentHits < maxHits)
            {
                Damageable damaged = hit.GetComponent<Damageable>();

                if (damaged != null && !targetsHit.Contains(hit.gameObject))
                {
                    damaged.TakeDamage(Mathf.RoundToInt(damage)); // ✅ FIX: Convert float to int
                    targetsHit.Add(hit.gameObject);
                    currentHits++;
                }
            }
        }

        if (currentHits >= maxHits) DestroyAttack();
    }

    // Handles attack destruction
    private void DestroyAttack()
    {
        Destroy(gameObject);
    }

    // Debugging: Show attack range in Unity Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
