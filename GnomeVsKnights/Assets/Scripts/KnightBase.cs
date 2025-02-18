using UnityEngine;

public class KnightBase : CharacterBase
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    private bool isMoving = true;

    [Header("Attack")]
    [SerializeField] private float attackRange = 0.5f;
    //  [SerializeField] private new float attackCooldown = 1f;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    [Header("Health")]
    [SerializeField] private float attackAnimDuration = 0.5f;

    protected override void Start()
    {
        base.Start();
        animator.SetBool("isWalking", true); // Start walking by default
    }

    protected override void Update()
    {
        base.Update();

        if (!isAttacking)
        {
            DetectGnome();
        }

        if (isMoving)
        {
            Move();
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void Move()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime; // Move left

        Vector3Int currentCell = GameManager.Instance.map.WorldToCell(transform.position);
        if (currentCell.x <= -1 && currentCell.y >= 1 && currentCell.y <= 5)
        {
            GameManager.Instance.KnightReachedEnd();
            Destroy(gameObject);
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void DetectGnome()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, targetLayer);
        if (hit != null)
        {
            isMoving = false;
            isAttacking = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
            Attack(hit.gameObject);
        }
        else
        {
            isMoving = true;
            isAttacking = false;
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
    }

    protected override void Attack(GameObject target)
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        isAttacking = true;
        base.Attack(target);

        if (target.TryGetComponent(out GnomeBase gnome))
        {
            gnome.TakeDamage(attackDamage);

            if (gnome.Health <= 0) // ✅ Now this will work!
            {
                Vector3Int gnomeCell = GameManager.Instance.map.WorldToCell(target.transform.position);
                GameManager.Instance.KillGnome(gnomeCell);
            }
        }

        Invoke(nameof(EndAttack), attackAnimDuration);
    }



    private void EndAttack()
    {
        isAttacking = false;
        isMoving = true;
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (animator != null)
        {
            animator.SetTrigger("isHit"); // Play hurt animation
        }

        Invoke(nameof(ResetHitState), 0.5f); // After hit animation, return to state
    }

    private void ResetHitState()
    {
        animator.SetBool("isHit", false);
    }

    protected override void Death()
    {
        isMoving = false;
        isAttacking = false;
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);

        Destroy(gameObject, 1f); // Delay destruction for animation
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
