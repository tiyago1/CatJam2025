using _Game.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Game.Scripts
{
    public class FoodSupplyController : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Food food;
        
        [Inject] private PlayerController player;
        [Inject] private SoundContoller soundContoller;

        public void OnPointerDown(PointerEventData eventData)
        {
            player.SetActiveFood(food);
            soundContoller.PlayClickSound();
            player.ChangeRequest(RequestType.Food);
        }
    }
}