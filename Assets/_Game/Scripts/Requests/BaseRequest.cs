using System;
using _Game.Enums;
using _Game.Signals;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Game.Scripts
{
    public abstract class BaseRequest : MonoBehaviour, IDisposable
    {
        [SerializeField] protected RequestView requestView;

        [Inject] private DataController _dataController;
        [Inject] protected PlayerController Player;
        [Inject] protected SignalBus SignalBus;
        [Inject] protected SoundContoller SoundContoller;

        public RequestType Type;
        protected Cat Cat;
        protected bool IsRequestIgnored;
        private Tweener _timer;

        public virtual void Initialize(Cat cat)
        {
            Cat = cat;
            requestView.StartTimer( GetDuration());
            StartTimer();
        }

        private void StartTimer()
        {
            _timer = DOVirtual.Float(0, 1, _dataController.Day.GetRequestDuration(),
                    requestView.ProgressBar.SetFillAmount)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Cat.Decline().Forget();
                    SignalBus.Fire<GameSignals.OnFailRequest>();
                })
                .SetDelay(.1f);
        }

        protected void CancelTimer()
        {
            _timer.Kill();
        }

        public abstract void Solve();

        public bool IsValid() => Player.ActiveRequest == Type;

        public virtual float GetDuration()
        {
            return _dataController.Day.GetRequestDuration();
        }

        public void Dispose()
        {
        }
    }
}