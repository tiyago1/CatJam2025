using System;
using _Game.Enums;
using UnityEngine;

namespace _Game.Scripts
{
    public class Cat : MonoBehaviour
    {
        public AreaType AreaType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dark"))
            {
                AreaType = AreaType.Dark;
            }
            
            if (other.CompareTag("Main"))
            {
                AreaType = AreaType.Main;
            }
            
            if (other.CompareTag("Garden"))
            {
                AreaType = AreaType.Garden;
            }
            
            if (other.CompareTag("Outside"))
            {
                AreaType = AreaType.Outside;
            }
        }
    }
}