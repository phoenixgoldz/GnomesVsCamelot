using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int health = 50;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject); 
    }
}