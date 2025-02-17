using UnityEngine;

public class RangedAttackBase : AttackBase
{
    [Header("Projectile Settings")]
    [SerializeField] protected float projectileSpeed = 5f;
    [SerializeField] protected GameObject attackFXPrefab; // FX prefab for attack visuals

    private SpriteRenderer spriteRenderer;
    private bool hasHitTarget = false;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Flip sprite if shooting left
        if (projectileSpeed < 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = true;
        }
    }

    protected override void FixedUpdate()
    {
        if (!hasHitTarget)
        {
            transform.position += Vector3.right * (projectileSpeed * Time.fixedDeltaTime);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable enemy) && !hasHitTarget)
        {
            enemy.TakeDamage(Mathf.RoundToInt(damage)); // Deal damage

            hasHitTarget = true; // Prevents multiple hits

            if (attackFXPrefab != null)
            {
                Instantiate(attackFXPrefab, transform.position, Quaternion.identity); // Spawn attack FX
            }

            Destroy(gameObject); // Destroy projectile after hit
        }
    }
}
