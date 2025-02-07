using UnityEngine;

public class CharacterBase : MonoBehaviour, Damageable
{
    // -1 means no range, 10 means full map range
    [SerializeField] protected int Range;
    //[SerializeField] protected static float Damage;

    [SerializeField] protected float Health;
    protected float currentHealth;

    //[SerializeField] protected static float PlacementDelay;
    //protected float PlacementCooldown; // used to track the time before Placement Cooldown is zero

    // -1 means it won't attack
    [SerializeField] protected float AttackRate;
    protected float AttackCooldown; // used to track the time before Attack Rate is zero

    [SerializeField] protected Transform RayOrigin;
    [SerializeField] protected LayerMask Targets;
    [SerializeField] protected GameObject AttackObject;
    [SerializeField] protected Animator animator;
    protected Vector3 RayDirection;

    protected virtual void Start()
    {
        currentHealth = Health;
    }

    protected virtual void FixedUpdate()
    {
        //if (PlacementCooldown > 0) PlacementCooldown -= Time.fixedDeltaTime; // Cut for now because I'm not sure if this is where it will end up being
        if (AttackRate != -1 && AttackCooldown > 0) AttackCooldown -= Time.fixedDeltaTime;
        if (AttackCooldown <= 0 && AttackRate != -1)
        {
            // change the equation to match the grid size
            float RayDistance = Range * 2;
            if (Physics2D.Raycast(RayOrigin.position, RayDirection, RayDistance, Targets))
            {
                Attack();
            }
        }
    }

    protected virtual void Attack()
    {
        Instantiate(AttackObject);
        AttackCooldown = AttackRate;
    }

    public virtual void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Death();
    }

    protected virtual void Death()
    {

    }
}
