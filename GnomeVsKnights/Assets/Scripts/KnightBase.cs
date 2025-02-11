using UnityEngine;

public class KnightBase : CharacterBase
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f; 

    [Header("Attack")]
    //[SerializeField] private int attackDamage = 10;
    //[SerializeField] private float attackCooldown = 1f;
    //[SerializeField] private float attackRange = 0.5f;
    //[SerializeField] private LayerMask targetLayer;

    [Header("Health")]
    //[SerializeField] private int maxHealth = 50;
    //private int currentHealth;

    private float attackTimer = 0f;
    [SerializeField] private float attackAnimDuration = 0.5f;
    private bool isAttacking = false;
    //private Animator animator;

    protected override void Start()
    {
        currentHealth = health;
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("IsWalking", true); 
        }
    }

    protected override void Update()
    {
        base.Update();

        if (!isAttacking && Physics2D.Raycast(rayOrigin.position, rayDirection, 0.75f, targetLayer).collider == null)
        {
            Move();
        }

        //if (attackTimer > 0)
        //{
        //    attackTimer -= Time.deltaTime;
        //}

        //DetectAndAttackTargets();
    }

    private void Move()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime; // Move left

        // Check if Knight reaches losing tilemap position (0,1-5,0)
        Vector3Int currentCell = GameManager.Instance.map.WorldToCell(transform.position);
        if (currentCell.x <= -1 && currentCell.y >= 1 && currentCell.y <= 5)
        {
            GameManager.Instance.KnightReachedEnd(); // Triggers Game Over
            Destroy(gameObject);
        }

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    protected override void Attack(GameObject target)
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }
        isAttacking = true;
        base.Attack(target);
        Invoke(nameof(EndAttack), attackAnimDuration); // Attack animation duration
    }

    //private void DetectAndAttackTargets()
    //{
    //    Collider2D hitTarget = Physics2D.OverlapCircle(transform.position, range, targetLayer);

    //    if (hitTarget != null && attackTimer <= 0)
    //    {
    //        isAttacking = true;

    //        if (animator != null)
    //        {
    //            animator.SetBool("IsWalking", false);
    //            animator.SetBool("IsAttacking", true);
    //        }

    //        Damageable target = hitTarget.GetComponent<Damageable>();
    //        if (target != null)
    //        {
    //            target.TakeDamage(attackDamage);
    //        }

    //        attackTimer = attackCooldown;
    //        Invoke(nameof(EndAttack), attackAnimDuration); // Attack animation duration
    //    }
    //}

    private void EndAttack()
    {
        isAttacking = false;

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttacking", false);
        }
    }

    //public void TakeDamage(int damage)
    //{
    //    currentHealth -= damage;

    //    if (currentHealth <= 0)
    //    {
    //        Die();
    //    }
    //}

    protected override void Death()
    {
        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }

        Destroy(gameObject, 0.5f); // Allow death animation to play before removal
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(rayOrigin.position, rayOrigin.position + (rayDirection * 0.75f));
    }
}
