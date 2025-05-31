using System.Collections.Generic;
using _Game.Data;
using _Game.Scripts.Data;
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
            get
            {
                return PlayerPrefs.GetInt("Day", 0);
            }
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
            // return Cats[Random.Range(0, Cats.Count)];
            return Cats[2];
        }
    }
}