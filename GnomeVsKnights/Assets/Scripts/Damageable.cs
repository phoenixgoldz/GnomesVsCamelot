using UnityEngine;

public interface Damageable// : MonoBehaviour
{
    //[SerializeField] private int health = 50;

    public void TakeDamage(int damage);
    //{
    //    health -= damage;
    //
    //    if (health <= 0)
    //    {
    //        Die();
    //    }
    //}

    //protected void Death()
    //{
    //    Destroy(gameObject); 
    //}
}