using UnityEngine;

public class GnomeBase : MonoBehaviour
{
    // -1 means no range, 10 means full map range
    [SerializeField] protected static int Range;
    [SerializeField] protected static float Damage;
    [SerializeField] protected static int Cost;

    [SerializeField] protected static float Health;
    protected float currentHealth;

    [SerializeField] protected static float PlacementDelay;
    //protected float PlacementCooldown; // used to track the time before Placement Cooldown is zero

    // -1 means it won't attack
    [SerializeField] protected static float AttackRate;
    protected float AttackCooldown; // used to track the time before Attack Rate is zero

    [SerializeField] protected Transform RayOrigin;
    [SerializeField] protected LayerMask Enemies;
    protected void FixedUpdate()
    {
        //if (PlacementCooldown > 0) PlacementCooldown -= Time.fixedDeltaTime; // Cut for now because I'm not sure if this is where it will end up being
        if (AttackRate != -1 && AttackCooldown > 0) AttackCooldown -= Time.fixedDeltaTime;
        if (AttackCooldown <= 0)
        {
            RaycastHit hit;
            // change the equation to match the grid size
            float RayDistance = Range * 3;
            if (Physics.Raycast(RayOrigin.position, Vector3.right, out hit, RayDistance, Enemies))
            {
                Attack();
            }
        }
    }

    protected void Attack()
    {

    }
}
