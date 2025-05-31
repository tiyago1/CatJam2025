using System.Collections.Generic;
using _Game.Scripts.Data;
using UnityEngine;

namespace _Game.Scripts
{
    public class DataController : MonoBehaviour
    {
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
    }
}