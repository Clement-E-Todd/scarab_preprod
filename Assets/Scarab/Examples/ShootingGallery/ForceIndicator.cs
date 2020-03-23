using UnityEngine;

namespace SlingshotGallery
{
    public class ForceIndicator : MonoBehaviour
    {
        public SlingshotElastic elastic;

        public SpriteRenderer arrowHead;

        private const float arrowLengthScale = 0.001f;

        void Update()
        {
            arrowHead.enabled = elastic.currentProjectile != null;

            if (arrowHead.enabled)
            {
                Vector2 launchForce = elastic.GetLaunchForce();
                Vector2 launchDirection = launchForce.normalized;

                float angle = Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                arrowHead.transform.localPosition = Vector3.right * launchForce.magnitude * arrowLengthScale;
            }
        }
    }
}