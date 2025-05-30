using _Game.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public abstract class BaseRequest : MonoBehaviour
    {
        protected PlayerController Player;
        public RequestType Type;
        private Cat Cat;

        public virtual void Initialize(Cat cat)
        {
            Cat = cat;

        }
        public abstract void Solve();

        public bool IsValid() => Player.ActiveRequest == Type;
    }

    public class FoodRequest : BaseRequest, IPointerDownHandler
    {
        [SerializeField] private FoodRequestView view;
        public FoodType FoodType;

        public override void Initialize(Cat cat)
        {
            base.Initialize(cat);
            
            FoodType = (FoodType)Random.Range(0, 3);
            view.Initialize(FoodType);
        }

        public override void Solve()
        {
            Destroy(Player.ActiveFood);
            Player.ActiveFood = null;
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