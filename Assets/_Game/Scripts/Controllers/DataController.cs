using System;
using System.Collections.Generic;
using _Game.Data;
using _Game.Scripts.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
               ClearData();
            }
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

        public int GetTotalPositiveCount()
        {
            int result = 0;
            for (int i = 0; i <= DayIndex; i++)
            {
                result += GetPositiveCount(i);
            }
            
            return result;
        }
        
        public int GetTotalNegativeCount()
        {
            int result = 0;
            for (int i = 0; i <= DayIndex; i++)
            {
                result += GetNegativeCount(i);
            }
            
            return result;
        }


        [Button]
        public void ClearData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}