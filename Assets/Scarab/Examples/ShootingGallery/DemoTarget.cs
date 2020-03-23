using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlingshotGallery
{
    public class DemoTarget : MonoBehaviour
    {
        public Rect spawnArea;

        private void OnEnable()
        {
            Respawn();
        }

        private void Respawn()
        {
            transform.position = new Vector3(
                (spawnArea.x - spawnArea.width / 2) + (spawnArea.width * Random.value),
                (spawnArea.y - spawnArea.height / 2) + (spawnArea.height * Random.value),
                0);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Respawn();
        }
    }
}