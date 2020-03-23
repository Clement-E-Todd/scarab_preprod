using Lean.Touch;
using UnityEngine;

namespace SlingshotGallery
{
    public class DemoProjectile : SlingshotProjectile
    {
        public SlingshotElastic slingshot;

        public bool reloadOnTap = false;

        public float maxReloadOnTapSpeed = 0.2f;

        public float minReloadOnTapTime = 1.5f;

        private float lastLaunchTime = 0f;

        private void OnEnable()
        {
            LeanTouch.OnFingerTap += OnTap;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerTap -= OnTap;
        }

        private void Reset()
        {
            slingshot.currentProjectile = this;
            body.isKinematic = true;
        }

        private void OnBecameInvisible()
        {
            Reset();
        }

        private void Update()
        {
            if (body.isKinematic)
            {
                body.velocity = Vector2.zero;
            }
            else if (body.IsSleeping())
            {
                Reset();
            }
        }

        public override void Launch(Vector2 force)
        {
            base.Launch(force);

            lastLaunchTime = Time.unscaledTime;
        }

        private void OnTap(LeanFinger finger)
        {
            float timeSinceLaunch = Time.unscaledTime - lastLaunchTime;

            if (reloadOnTap && slingshot.currentProjectile != this && timeSinceLaunch >= minReloadOnTapTime && body.velocity.magnitude <= maxReloadOnTapSpeed)
            {
                Reset();
            }
        }
    }
}