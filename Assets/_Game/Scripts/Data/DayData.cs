using UnityEngine;

namespace _Game.Scripts.Data
{
    [CreateAssetMenu(fileName = "Day", menuName = "Data")]
    public class DayData : ScriptableObject
    {
        public int CatCount;
        public float Duration;
        public float FailStress;
        public float SuccessStress;
        public float MaxStress;
        public float TimeIncreaseStress;
        
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