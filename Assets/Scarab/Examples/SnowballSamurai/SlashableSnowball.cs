using UnityEngine;

namespace SnowballSamurai
{
    public class SlashableSnowball : MonoBehaviour, ISwipeSlashable
    {
        public GameObject snowballPrefab;
        public CircleCollider2D circleCollider;

        private const float MinScale = 0.1f;
        private const float InvulverabilityDuration = 0.1f;
        private const float Gravity = 9.8f;
        private const float VelocityOnSlash = 4f;

        private float invulnerabilityStartTime;

        private Vector2 velocity;

        private void OnEnable()
        {
            invulnerabilityStartTime = Time.unscaledTime;
        }

        private void Start()
        {
            if (transform.localScale.x < MinScale)
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            velocity += Vector2.left * Gravity * Time.fixedDeltaTime;
            transform.position += (Vector3)velocity * Time.fixedDeltaTime;
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        public void AddForce(Vector2 force)
        {
            velocity += force;
        }

        void ISwipeSlashable.OnSwipeSlash(Vector2 start, Vector2 end)
        {
            // If the snowball is invulnerable (ie. immediately after it is spawned), ignore the slash
            if (Time.unscaledTime < invulnerabilityStartTime + InvulverabilityDuration)
            {
                return;
            }

            // Calculate the closest point along the slash line to the center of the snow ball
            Vector2 center = (Vector2)transform.position;
            Vector2 direction = (end - start).normalized;
            Vector2 startToCenter = (Vector2)transform.position - start;
            Vector2 closestPoint = start + Vector3.Dot(startToCenter, direction) * direction;

            // Calculate how cleanly the snowball should be cut in half based on how close the
            // slash passed by the center point
            float distanceFromCenter = (closestPoint - center).magnitude;
            float scaledRadius = circleCollider.radius * transform.localScale.x;
            float scaleOfSmallerNewSnowball = (0.5f - (distanceFromCenter / scaledRadius) / 2) * transform.localScale.x;

            // Create two new, smaller snowballs on either side of the slash
            if (scaleOfSmallerNewSnowball > 0f)
            {
                // Instantiate the snowballs and get their snowball components
                GameObject smallSnowBallObject = Instantiate(snowballPrefab);
                GameObject largeSnowBallObject = Instantiate(snowballPrefab);
                SlashableSnowball smallSnowBall = smallSnowBallObject.GetComponent<SlashableSnowball>();
                SlashableSnowball largeSnowBall = largeSnowBallObject.GetComponent<SlashableSnowball>();

                // Set the scale of the two new snowballs based on where the original snowball was slashed
                smallSnowBall.transform.localScale = Vector3.one * scaleOfSmallerNewSnowball;
                largeSnowBall.transform.localScale = Vector3.one * (transform.localScale.x - scaleOfSmallerNewSnowball);

                // Set the positions of the two new snowballs based on where the original snowball was slashed
                Vector2 directionFromCenter = (closestPoint - center).normalized;
                smallSnowBall.transform.position = closestPoint + directionFromCenter * smallSnowBall.circleCollider.radius * smallSnowBall.transform.localScale.x;
                largeSnowBall.transform.position = closestPoint - directionFromCenter * largeSnowBall.circleCollider.radius * largeSnowBall.transform.localScale.x;

                // Set the velocity of the two new snowballs based on where the original snowball was slashed
                smallSnowBall.AddForce(velocity + directionFromCenter * VelocityOnSlash);
                largeSnowBall.AddForce(velocity - directionFromCenter * VelocityOnSlash);
            }

            // This snowball, now cut in two, should be destroyed
            Destroy(gameObject);
        }
    }
}