using UnityEngine;

public class GnomeBase : CharacterBase
{
    //[SerializeField] protected static int Cost;

    protected override void FixedUpdate()
    {
        RayDirection = Vector3.right;
        base.FixedUpdate();
    }

    public void OnDrawGizmosSelected()
    {
        //RaycastHit hit;
        float RayDistance = Range * 2;
        Vector3 direction = transform.TransformDirection(Vector3.right) * RayDistance;
        //Physics.Raycast(RayOrigin.position, RayDirection, out hit, RayDistance, Targets);
        Gizmos.DrawRay(RayOrigin.position, direction);
    }
}
