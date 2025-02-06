using UnityEngine;

public class KnightBase : CharacterBase
{
    [SerializeField] protected static float MoveSpeed;

    protected override void FixedUpdate()
    {
        RayDirection = Vector3.left;
        base.FixedUpdate();
        transform.position = new Vector3(transform.position.x - (MoveSpeed * Time.fixedDeltaTime), transform.position.y, transform.position.z);
    }
}