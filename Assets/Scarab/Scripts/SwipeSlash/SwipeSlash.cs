using UnityEngine;
using Lean.Touch;

public class SwipeSlash : MonoBehaviour
{
    public Camera overrideCamera;

    public float minSlashSpeed = 5f;

    LeanFinger finger;

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
        }
    }

    public void OnFingerUp(LeanFinger finger)
    {
        if (this.finger == finger)
        {
            this.finger = null;
        }
    }

    private void Update()
    {
        if (finger != null)
        {
            Camera camera = overrideCamera ? overrideCamera : Camera.main;

            Vector3 lastPosition = finger.GetLastWorldPosition(Mathf.Abs(camera.transform.position.z), camera);
            Vector3 currentPosition = finger.GetWorldPosition(Mathf.Abs(camera.transform.position.z), camera);
            float swipeSpeed = (currentPosition - lastPosition).magnitude / Time.deltaTime;

            if (swipeSpeed >= minSlashSpeed)
            {
                OnSlash(lastPosition, currentPosition);
            }
        }
    }

    private void OnSlash(Vector2 start, Vector2 end)
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(start, end);
        for (int i = 0; i < hits.Length; i++)
        {
            ISwipeSlashable slashable = hits[i].collider.GetComponent<ISwipeSlashable>();
            if (slashable != null)
            {
                slashable.OnSwipeSlash(start, end);
            }
        } 
    }
}
