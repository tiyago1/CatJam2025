using UnityEngine;

namespace _Game.Scripts
{
    public class DataController : MonoBehaviour
    {
        public float MinRequestDuration = 2;
        public float MaxRequestDuration = 5;
        
        public float MinWaitRequestDuration = 2;
        public float MaxWaitRequestDuration = 5;

        public float GetRequestDuration()
        {
            return Random.Range(MinRequestDuration, MaxRequestDuration);
        }
        
        public float GetRequestWaitDuration()
        {
            return Random.Range(MinWaitRequestDuration, MaxWaitRequestDuration);
        }
    }
}