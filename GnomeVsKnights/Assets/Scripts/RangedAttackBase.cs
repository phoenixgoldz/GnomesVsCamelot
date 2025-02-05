using UnityEngine;

public class RangedAttackBase : AttackBase
{
    [SerializeField] protected float ProjectileSpeed;
    [SerializeField] protected float ProjectileLifespan;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (ProjectileLifespan <= 0) Destroy(this);
        transform.position = new Vector3(transform.position.x + (ProjectileSpeed * Time.fixedDeltaTime), transform.position.y, transform.position.z);
    }
}
