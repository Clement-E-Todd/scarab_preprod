using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlingshotProjectile : MonoBehaviour
{
    public Rigidbody2D body;

    public virtual void Launch(Vector2 force)
    {
        body.isKinematic = false;
        body.velocity = Vector2.zero;
        body.AddForce(force);
    }
}
