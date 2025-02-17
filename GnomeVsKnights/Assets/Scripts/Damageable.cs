using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
}

public class Damageable : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 50;
    private int currentHealth;

    private Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (animator != null)
        {
            animator.SetTrigger("isHit"); // Trigger hit animation if available
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
            animator.SetTrigger("isDead"); // Play death animation
        }

        Destroy(gameObject, 0.5f); // Delay destruction to allow animation to play
    }
}
