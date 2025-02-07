using UnityEngine;

public class KnightBase : CharacterBase
{
    [SerializeField] protected float MoveSpeed;

    protected override void FixedUpdate()
    {
        RayDirection = Vector3.left;
        base.FixedUpdate();
        transform.position = new Vector3(transform.position.x - (MoveSpeed * Time.fixedDeltaTime), transform.position.y, transform.position.z);
    }


    public void OnDrawGizmosSelected()
    {
        //RaycastHit hit;
        float RayDistance = Range * 2;
        Vector3 direction = transform.TransformDirection(Vector3.left) * RayDistance;
        //Physics.Raycast(RayOrigin.position, RayDirection, out hit, RayDistance, Targets);
        Gizmos.DrawRay(RayOrigin.position, direction);
    }
}