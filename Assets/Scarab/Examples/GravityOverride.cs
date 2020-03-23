using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOverride : MonoBehaviour
{
    public Vector2 gravity;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.gravity = gravity;
    }
}
