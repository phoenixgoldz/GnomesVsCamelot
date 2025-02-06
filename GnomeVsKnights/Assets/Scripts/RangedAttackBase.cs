using UnityEngine;

public class RangedAttackBase : AttackBase
{
    [SerializeField] protected float ProjectileSpeed;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        transform.position = new Vector3(transform.position.x + (ProjectileSpeed * Time.fixedDeltaTime), transform.position.y, transform.position.z);
    }
}
