using _Game.Signals;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts
{
    public class LoveRequest : BaseRequest, IPointerClickHandler
    {
        [SerializeField] private LoveRequestView view;

        [SerializeField] private int clickMaxCount = 3;
        [SerializeField] private int current = 3;

        [Button]
        public override void Initialize(Cat cat)
        {
            view.Initialize();

            base.Initialize(cat);
        }

        public override void Solve()
        {
            CancelTimer();

            Cat.Approved().Forget();
            SignalBus.Fire<GameSignals.OnSuccessRequest>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsRequestIgnored)
                return;

            current++;
            var value = ReRangeValue(current, 0, clickMaxCount, 0, 1);
            view.FillAmountValue(value);
            if (current >= clickMaxCount)
            {
                Solve();
            }
        }

        private float ReRangeValue(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
        }
    }
}