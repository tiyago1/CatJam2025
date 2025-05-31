using System;
using _Game.Signals;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        private void Awake()
        {
            StartGame();
        }

        public void StartGame()
        {
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
        }

        private void OnFailRequest(GameSignals.OnFailRequest obj)
        {
            IncreaseStress(_dataController.Day.FailStress);
        }

        private void StartDayTimer()
        {
            DOVirtual.Float(0, 1, _dataController.Day.Duration,
                    (normalizedValue) => { _gameUIController.SetDayIndicator(normalizedValue); })
                .OnComplete(FailLevel);
            GetStress().Forget();
        }

        private async UniTask GetStress()
        {
            for (int i = 0; i < (int)_dataController.Day.Duration; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                IncreaseStress(_dataController.Day.TimeIncreaseStress);
            }
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

        public void ClearLevel()
        {
            _dataController.DayIndex++;

            if (_dataController.DayIndex >= 6)
            {
                // High Score goster

                // _dataController.DayIndex = 0;
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

        public void GameOver()
        {
            Debug.Log("GameOver");
        }

        public void Dispose()
        {
            UnsubscribeSignals();
        }
    }
}