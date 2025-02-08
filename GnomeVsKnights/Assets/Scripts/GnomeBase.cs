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
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (animator != null)
        {
            animator.SetBool("isAttacking", true); // Example: Start attack animation
        }
    }

    public override void TakeDamage(int damage) // Override the base method
    {
        base.TakeDamage(damage); // Call the base logic (reduce health)
        if (animator != null)
        {
            animator.SetTrigger("isHurt"); // Play hurt animation
        }
    }

    protected override void Death() // Optional: Override death logic if needed
    {
        if (animator != null)
        {
            animator.SetBool("isDead", true); // Trigger death animation
        }

        Destroy(gameObject, 1f); // Destroy with delay to allow animation to play
    }
}
