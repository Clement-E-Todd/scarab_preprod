using Lean.Touch;
using UnityEngine;

public class SlingshotElastic : MonoBehaviour
{
    public Camera overrideCamera;

    public SlingshotProjectile currentProjectile;

    public float minStretchDistance = 0f;
    public float maxStretchDistance = 2.5f;

    public float projectileLaunchForce = 5000f;

    [Range(0f, 1f)]
    public float minLaunchTension = 0.25f;

    [Range(0f, 1f)]
    public float jiggleIntensity = 1f;

    [Range(0f, 1f)]
    public float jiggleDamping = 0.1f;

    private LeanFinger finger;

    private Vector3 slingPositionOnDown;
    private Vector3 touchPositionOnDown;

    private Vector3 jiggleVelocity;

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }

    public void OnFingerDown(LeanFinger finger)
    {
        if (this.finger == null)
        {
            this.finger = finger;

            slingPositionOnDown = transform.localPosition;

            Camera camera = overrideCamera != null ? overrideCamera : Camera.main;
            touchPositionOnDown = finger.GetWorldPosition(Mathf.Abs(camera.transform.position.z), camera);
        }
    }

    public void OnFingerUp(LeanFinger finger)
    {
        if (this.finger == finger)
        {
            this.finger = null;

            if (currentProjectile && GetTension() >= minLaunchTension)
            {
                currentProjectile.Launch(GetLaunchForce());
                currentProjectile = null;
            }
        }
    }

    private void Update()
    {
        if (finger != null)
        {
            Camera camera = overrideCamera != null ? overrideCamera : Camera.main;
            Vector3 touchPosition = finger.GetWorldPosition(Mathf.Abs(camera.transform.position.z), camera);
            Vector3 delta = touchPosition - touchPositionOnDown;
            transform.localPosition = slingPositionOnDown + delta;
            if (transform.localPosition.magnitude > maxStretchDistance)
            {
                transform.localPosition = transform.localPosition.normalized * maxStretchDistance;
            }
            jiggleVelocity = Vector3.zero;
        }
        else
        {
            float tension = GetTension();
            if (tension > 0f)
            {
                jiggleVelocity -= transform.localPosition.normalized * tension * projectileLaunchForce * jiggleIntensity * Time.deltaTime;
            }

            float decceleration = projectileLaunchForce * jiggleDamping;

            if (jiggleVelocity.magnitude > decceleration * Time.deltaTime)
            {
                jiggleVelocity -= jiggleVelocity.normalized * decceleration * Time.deltaTime;
            }
            else
            {
                jiggleVelocity = Vector3.zero;
            }

            transform.position += jiggleVelocity * Time.deltaTime;
        }

        if (currentProjectile != null)
        {
            currentProjectile.transform.position = transform.position;
        }
    }

    public Vector2 GetLaunchForce()
    {
        return -transform.localPosition.normalized * projectileLaunchForce * GetTension();
    }

    private float GetTension()
    {
        return Mathf.Clamp01((transform.localPosition.magnitude - minStretchDistance) / (maxStretchDistance - minStretchDistance));
    }
}
