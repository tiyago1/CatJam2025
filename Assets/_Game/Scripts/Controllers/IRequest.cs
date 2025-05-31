using System;
using _Game.Enums;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Game.Scripts
{
    public abstract class BaseRequest : MonoBehaviour
    {
        [SerializeField] protected RequestView requestView;

        [Inject] private DataController _dataController;
        [Inject] protected PlayerController Player;

        public RequestType Type;
        protected Cat Cat;
        private Tweener _timer;

        public virtual void Initialize(Cat cat)
        {
            Cat = cat;
            requestView.StartTimer(_dataController.Day.GetRequestDuration());
            StartTimer();
        }

        private void StartTimer()
        {
           _timer=  DOVirtual.Float(0, 1, _dataController.Day.GetRequestDuration(), requestView.ProgressBar.SetFillAmount)
                .SetEase(Ease.Linear)
                .OnStepComplete(() => OnTimeCompleted().Forget())
                .SetDelay(.1f);
        }

        protected void CancelTimer()
        {
            _timer.Kill();
        }

        protected virtual async UniTask OnTimeCompleted()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(.2f));
            await UniTask.Delay(TimeSpan.FromSeconds(_dataController.Day.GetRequestWaitDuration()));
            Cat.OnTimeCompleted();
        }

        public abstract void Solve();

        public bool IsValid() => Player.ActiveRequest == Type;
    }
}