using UnityEngine;

namespace SnowballSamurai
{
    public class SnowballSpawner : MonoBehaviour
    {
        public GameObject snowballPrefab;

        public Rect spawnArea;
        public Rect targetArea;

        public const float TimeBeforeFirstSpawn = 1f;

        public const float MinTimeBetweenSpawns = 0.25f;
        public const float MaxTimeBetweenSpawns = 2f;

        public const float MinForce = 11f;
        public const float MaxForce = 13f;

        private float nextSpawnTime = 0f;

        private void Awake()
        {
            SetTimeToNextSpawn(TimeBeforeFirstSpawn);
        }

        private void Update()
        {
            if (Time.time >= nextSpawnTime)
            {
                Spawn();
                SetTimeToNextSpawn(MinTimeBetweenSpawns + (MaxTimeBetweenSpawns - MinTimeBetweenSpawns) * Random.value);
            }
        }

        private void Spawn()
        {
            GameObject snowballObject = Instantiate(snowballPrefab);
            SlashableSnowball snowball = snowballObject.GetComponent<SlashableSnowball>();

            snowballObject.transform.position = new Vector3(
                spawnArea.x - (spawnArea.width / 2) + spawnArea.width * Random.value,
                spawnArea.y - (spawnArea.height / 2) + spawnArea.height * Random.value);

            Vector2 destination = new Vector3(
                targetArea.x - (targetArea.width / 2) + targetArea.width * Random.value,
                targetArea.y - (targetArea.height / 2) + targetArea.height * Random.value);
            Vector2 direction = (destination - (Vector2)snowballObject.transform.position).normalized;
            snowball.AddForce(direction * (MinForce + (MaxForce - MinForce) * Random.value));
        }

        private void SetTimeToNextSpawn(float delay)
        {
            nextSpawnTime = Time.time + delay;
        }
    }
}