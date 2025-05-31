using _Game.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Game.Scripts
{
    public class FoodSupplyController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Food food;
        
        [Inject] private PlayerController player;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");
            player.ChangeRequest(RequestType.Food);
            player.SetActiveFood(food);
        }
    }
}