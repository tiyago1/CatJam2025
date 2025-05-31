using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class RequestView : MonoBehaviour
    {
        public List<Sprite> Sprites;
        public Image Image;
        public CircularProgressBar ProgressBar; 
        
        public void StartTimer(float duration)
        {
            ProgressBar.Initialize();
            ProgressBar.SetFillAmount(duration);
        }
    }
}