using System.Collections.Generic;
using _Game.Data;
using _Game.Scripts.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts
{
    public class DataController : MonoBehaviour
    {
        public List<CatData> Cats;
        public List<DayData> Days;
        public DayData Day;

        public int DayIndex
        {
            get { return PlayerPrefs.GetInt("Day", 0); }
            set
            {
                PlayerPrefs.SetInt("Day", value);
                PlayerPrefs.Save();
            }
        }

        public void Initialize()
        {
            Day = Instantiate(Days[DayIndex]);
        }

        public CatData GetRandomCat()
        {
            return Cats[Random.Range(0, Cats.Count)];
        }

        public int GetPositiveCount(int dayIndex)
        {
            return PlayerPrefs.GetInt($"Day{dayIndex}Positive", 0);
        }

        public void SetPositiveCount(int dayIndex, int count)
        {
            PlayerPrefs.SetInt($"Day{dayIndex}Positive", count);
        }

        public int GetNegativeCount(int dayIndex)
        {
            return PlayerPrefs.GetInt($"Day{dayIndex}Negative", 0);
        }

        public void SetNegativeCount(int dayIndex, int count)
        {
            PlayerPrefs.SetInt($"Day{dayIndex}Negative", count);
        }

        [Button]
        public void ClearData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}