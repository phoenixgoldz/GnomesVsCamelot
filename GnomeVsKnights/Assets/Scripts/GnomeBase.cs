using UnityEngine;

public class GnomeBase : MonoBehaviour
{
    // -1 means no range, 10 means full map range
    [SerializeField] protected int Range;
    [SerializeField] protected float Health;
    [SerializeField] protected float Damage;

    [SerializeField] protected float AttackRate;
    protected float AttackCooldown; // used to track remaining time before newxt
}
