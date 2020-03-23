using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SliceTheString
{
    public class DestroyOnCollision : MonoBehaviour
    {
        public GameObject orb;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject == orb)
            {
                Destroy(gameObject);
            }
        }
    }
}