
using UnityEngine;

//Test purpose
[RequireComponent(typeof(Rigidbody))]
public class Target : NhoxBehaviour
{
    protected override void Start()
    {
        base.Start();
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
}
