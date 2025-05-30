using System.Collections.Generic;
using _Game.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class RequestView : MonoBehaviour
    {
        public List<Sprite> Sprites;
        public Image Image;
    }

    public class FoodRequestView : RequestView
    {
        public void Initialize(FoodType type)
        {
            Image.sprite = Sprites[(int)type];
        }
    }
}