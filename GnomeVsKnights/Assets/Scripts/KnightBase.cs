using UnityEngine;

public class KnightBase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f; 

    [Header("Attack")]
    [SerializeField] private int attackDamage = 10; 
    [SerializeField] private float attackRange = 0.5f; 
    [SerializeField] private float attackCooldown = 1f; 
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
    }

    private void Update()
    {
        if (isAttacking) return; 

        Move();

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        DetectAndAttackTargets();
    }

    private void Move()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

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
                animator.SetTrigger("Attack");
            }

            Damageable target = hitTarget.GetComponent<Damageable>();
            if (target != null)
            {
                target.TakeDamage(attackDamage); 
            }

            attackTimer = attackCooldown;

            Invoke(nameof(EndAttack), 0.5f);
        }
    }

    private void EndAttack()
    {
        isAttacking = false;

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (animator != null)
        {
            animator.SetTrigger("TakeDamage");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Destroy(gameObject, 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
