using UnityEngine;

public class GnomeBase : CharacterBase
{
    [Header("Gnome Settings")]
    [SerializeField] private int cost = 25;

    public Vector3Int Cell { get; set; }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>(); // Get Animator component
        SetIdleState();
    }

    protected override void Update()
    {
        base.Update();
        DetectEnemies();
    }

    private void DetectEnemies()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, range, targetLayer);

        if (enemy != null)
        {
            SetAttackState();
        }
        else
        {
            SetIdleState();
        }
    }

    private void SetIdleState()
    {
        if (animator != null)
        {
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsIdle", true);
        }
    }

    private void SetAttackState()
    {
        if (animator != null)
        {
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsAttacking", true);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (animator != null)
        {
            animator.SetTrigger("IsHurt"); // Play hurt animation
        }
    }

    protected override void Death()
    {
        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }
        Destroy(gameObject, 1f); // Allow animation to play before destroying
    }
}
