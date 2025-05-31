using _Game.Signals;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Game.Scripts
{
    public class GameUIController : MonoBehaviour
    {
        [SerializeField] private ClassicProgressBar stressSlider;
        [SerializeField] private RectTransform dayIndicator;
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private GameOverPanelView gameOverPanelView;
        [SerializeField] private DayFinishedPanelView dayFinishedPanel;
        
        [SerializeField] private float minRotation;
        [SerializeField] private float maxRotation;

        [Inject] private DataController _dataController;
        [Inject] private SignalBus _signalBus;

        public void Initialize()
        {
            SetDayIndicator(1);
            dayText.text = $"Day {_dataController.DayIndex + 1}";
            _signalBus.Subscribe<GameSignals.OnGameOver>(OnGameOver);
            _signalBus.Subscribe<GameSignals.OnNextDay>(ActivateDayFinishedPanel);
        }

        private void OnGameOver()
        {
            _dataController.ClearData();
        }

        [Button]
        public void SetDayIndicator(float value)
        {
            var rotation = new Vector3(0, 0, Mathf.Lerp(minRotation, maxRotation, 1 - value));
            dayIndicator.transform.DOLocalRotate(rotation, .5f);
        }

        [Button]
        public void SetStress(float value)
        {
            stressSlider.FillAmount = value;
            gameOverPanelView.SetGameOverDarkness(value);
        }

        public void ActivateDayFinishedPanel()
        {
            dayFinishedPanel.Show();
        }

        private float ReRangeValue(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
        }
    }
}