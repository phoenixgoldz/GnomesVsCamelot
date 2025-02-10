using UnityEngine;

public class KnightBase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f; 

    [Header("Attack")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask targetLayer;

    [Header("Health")]
    [SerializeField] private int maxHealth = 50;
    private int currentHealth;

    private float attackTimer = 0f;
    private bool isAttacking = false;
    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("IsWalking", true); 
        }
    }

    private void Update()
    {
        if (!isAttacking)
        {
            Move();
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        DetectAndAttackTargets();
    }

    private void Move()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime; // Move left

        // Check if Knight reaches losing tilemap position (0,1-5,0)
        Vector3Int currentCell = GameManager.Instance.map.WorldToCell(transform.position);
        if (currentCell.x == 0 && currentCell.y >= 1 && currentCell.y <= 5)
        {
            GameManager.Instance.KnightReachedEnd(); // Triggers Game Over
            Destroy(gameObject);
        }

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    private void DetectAndAttackTargets()
    {
        Collider2D hitTarget = Physics2D.OverlapCircle(transform.position, attackRange, targetLayer);

        if (hitTarget != null && attackTimer <= 0)
        {
            isAttacking = true;

            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsAttacking", true);
            }

            Damageable target = hitTarget.GetComponent<Damageable>();
            if (target != null)
            {
                target.TakeDamage(attackDamage);
            }

            attackTimer = attackCooldown;
            Invoke(nameof(EndAttack), 0.5f); // Attack animation duration
        }
    }

    private void EndAttack()
    {
        isAttacking = false;

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttacking", false);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }

        Destroy(gameObject, 0.5f); // Allow death animation to play before removal
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
