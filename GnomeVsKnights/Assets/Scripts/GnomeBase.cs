using TMPro;
using UnityEngine;

public class GnomeBase : CharacterBase
{
    [Header("Gnome Settings")]
    [SerializeField] private int cost = 25;
    [SerializeField] private TextMeshProUGUI ResourceCounter;

    private int CurrentResources;

    public Vector3Int Cell { get; set; }

    protected override void Start()
    {
        base.Start(); // Inherits initialization from CharacterBase
        SetIdleState();

        CurrentResources = int.Parse(ResourceCounter.text);
        CurrentResources -= cost;
        ResourceCounter.text = CurrentResources.ToString();
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
            animator.SetTrigger("Idle");
        }
    }

    private void SetAttackState()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (animator != null)
        {
            animator.SetTrigger("isHit"); // Play hurt animation
        }
    }

    protected override void Death()
    {
        if (animator != null)
        {
            animator.SetTrigger("isDead");
        }
        Destroy(gameObject, 1f); // Allow animation to play before destroying
    }
}
