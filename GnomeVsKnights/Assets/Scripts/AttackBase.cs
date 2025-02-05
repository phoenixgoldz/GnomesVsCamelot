using NUnit.Framework;
using System.Linq;
using UnityEngine;

public class AttackBase : MonoBehaviour
{
    [SerializeField] protected LayerMask Targets;
    [SerializeField] protected float HitRadius;
    [SerializeField] protected float Damage;
    [SerializeField] protected int HitCount = 1;
    protected int currentHits;
    private GameObject[] TargetsHit;

    void Start()
    {
        TargetsHit = new GameObject[HitCount];
    }

    protected virtual void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, HitRadius, Vector3.right, Mathf.Infinity, Targets);
        Damageable damaged = hit.collider.GetComponent<Damageable>();
        if (damaged != null && !TargetsHit.Contains(hit.collider.gameObject))
        {
            damaged.ApplyDamage(Damage);
            TargetsHit[currentHits] = hit.collider.gameObject;
            currentHits++;
        }

        if (currentHits >= HitCount) Destroy(this);
    }
}
