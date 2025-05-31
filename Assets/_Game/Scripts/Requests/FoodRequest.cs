using _Game.Enums;
using _Game.Signals;
using Cysharp.Threading.Tasks;
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
            if (Player.ActiveFood.Type == FoodType)
            {
                Cat.Approved().Forget();
                SignalBus.Fire<GameSignals.OnSuccessRequest>();
            }
            else
            {
                Cat.Decline().Forget();
                SignalBus.Fire<GameSignals.OnFailRequest>();
            }
            
            CancelTimer();
            Player.ClearFood();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsValid() || IsRequestIgnored)
                return;

            Solve();
        }
    }
}