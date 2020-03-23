using UnityEngine;

namespace SliceTheString
{
    public class StringSegment : MonoBehaviour, ISwipeSlashable
    {
        public void OnSwipeSlash(Vector2 start, Vector2 end)
        {
            Destroy(gameObject);
        }
    }
}