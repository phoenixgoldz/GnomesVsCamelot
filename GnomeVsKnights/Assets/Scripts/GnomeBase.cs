using UnityEngine;

public class GnomeBase : CharacterBase
{
    //[SerializeField] protected static int Cost;

    protected override void FixedUpdate()
    {
        RayDirection = Vector3.right;
        base.FixedUpdate();
    }
}
