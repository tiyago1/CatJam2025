using _Game.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts
{
    public class FoodRequest : BaseRequest, IPointerDownHandler
    {
        [SerializeField] private FoodRequestView view;
        public FoodType FoodType;

        [Button]
        public override void Initialize(Cat cat)
        {
            FoodType = (FoodType)Random.Range(0, 3);
            view.Initialize(FoodType);
            
            base.Initialize(cat);
        }

        public override void Solve()
        {
            CancelTimer();
            Player.ClearFood();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsValid())
                return;

            if (Player.ActiveFood.Type != FoodType)
                return;

            Solve();
        }
    }
}