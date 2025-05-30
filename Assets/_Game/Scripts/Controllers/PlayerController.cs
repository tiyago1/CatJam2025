using System;
using System.Collections.Generic;
using _Game.Enums;
using UnityEngine;

namespace _Game.Scripts
{
    public class PlayerController  : MonoBehaviour
    {
        public RequestType ActiveRequest;
        public Food ActiveFood;
        private Vector3 _position;

        private void Awake()
        {
            _position = Vector3.zero;
        }

        public void SetActiveFood(Food food)
        {
            ClearFood();
            
            ActiveFood = Instantiate(food, transform.position, Quaternion.identity);
            ActiveFood.transform.SetParent(this.transform);
        }

        public void ClearFood()
        {
            if (ActiveFood == null)
                return;
            
            Destroy(ActiveFood.gameObject);
            ActiveFood = null;
        }
        
        private void Update()
        {
            if (ActiveFood != null)
            {
                _position= Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _position.z = 0f;
                ActiveFood.transform.position = _position;
            }
        }
    }
}