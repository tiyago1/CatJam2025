using System;
using System.Threading;
using _Game.Signals;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Managers
{
    public class GameManager : MonoBehaviour, IDisposable
    {
        [Inject] private DataController _dataController;
        [Inject] private SpawnController _spawnController;
        [Inject] private GameUIController _gameUIController;
        [Inject] private SignalBus _signalBus;

        public float MaxStress;
        public float CurrentStress;
        public bool IsGameOver;
        
        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            StartGame();
        }

        public void StartGame()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _dataController.Initialize();
            _gameUIController.Initialize();
            _spawnController.Initialize().Forget();

            StartDayTimer();
            SubscribeSignals();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<GameSignals.OnSuccessRequest>(OnSuccessRequest);
            _signalBus.Subscribe<GameSignals.OnFailRequest>(OnFailRequest);
        }

        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<GameSignals.OnSuccessRequest>(OnSuccessRequest);
            _signalBus.Unsubscribe<GameSignals.OnFailRequest>(OnFailRequest);
        }

        private void OnSuccessRequest(GameSignals.OnSuccessRequest obj)
        {
            DecreaseStress(_dataController.Day.SuccessStress);
            var day = _dataController.DayIndex;
            _dataController.SetPositiveCount(day, _dataController.GetPositiveCount(day) + 1);
        }

        private void OnFailRequest(GameSignals.OnFailRequest obj)
        {
            IncreaseStress(_dataController.Day.FailStress);
            var day = _dataController.DayIndex;
            _dataController.SetNegativeCount(day, _dataController.GetNegativeCount(day) + 1);
        }

        private void StartDayTimer()
        {
            GetDayTimer().Forget();
            GetStress().Forget();
        }

        private async UniTask GetDayTimer()
        {
            for (int i = 0; i < _dataController.Day.Duration; i++)
            {
                float end = ReRangeValue(i+1, 0, _dataController.Day.Duration, 0, 1);
                float start = ReRangeValue(i, 0, _dataController.Day.Duration, 0, 1);
                DOVirtual.Float(start, end, 1, (seconds) =>
                {
                    _gameUIController.SetDayIndicator(seconds);
                });  
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: _cancellationTokenSource.Token);
            }

            if (_cancellationTokenSource.IsCancellationRequested || IsGameOver)
                return;
            
            OnDayTimeFinished();
        }

        private async UniTask GetStress()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: _cancellationTokenSource.Token);
                IncreaseStress(_dataController.Day.TimeIncreaseStress);
                if (CurrentStress >= MaxStress)
                {
                    break;
                }
            }
            
            if (_cancellationTokenSource.IsCancellationRequested || IsGameOver)
                return;

            GameOver();
        }

        [Button]
        public void IncreaseStress(float value)
        {
            CurrentStress = Mathf.Clamp(CurrentStress + value, 0, MaxStress);
            value = ReRangeValue(CurrentStress, 0, MaxStress, 0, 1);
            _gameUIController.SetStress(value);
            if (CurrentStress >= MaxStress)
            {
                GameOver();
            }
        }

        [Button]
        public void DecreaseStress(float value)
        {
            CurrentStress = Mathf.Clamp(CurrentStress - value, 0, MaxStress);
            value = ReRangeValue(CurrentStress, 0, MaxStress, 0, 1);
            _gameUIController.SetStress(value);
        }

        private float ReRangeValue(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
        }

        public void FailLevel()
        {
            _dataController.DayIndex = 0;
        }

        public void OnDayTimeFinished()
        {
            if (CurrentStress < MaxStress )
            {
                if (_dataController.DayIndex < 6)
                {
                    _signalBus.Fire<GameSignals.OnNextDay>();
                }
                else
                {
                    _signalBus.Fire<GameSignals.OnGameOverWithSuccess>();
                }
            }
            else 
            {
                GameOver();
            }
        }

        [Button]
        public void GameOver()
        {
            if (IsGameOver)
                return;
            
            Debug.Log("GameOver");
            IsGameOver = true;
            Dispose();
            _cancellationTokenSource.Cancel();
            _signalBus.Fire<GameSignals.OnGameOver>();
        }

        public void Dispose()
        {
            UnsubscribeSignals();
            _spawnController.Dispose();
        }
    }
}