using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Game.Scripts
{
    public class DayFinishedPanelView : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public TextMeshProUGUI dayText;
        public TextMeshProUGUI positiveText;
        public TextMeshProUGUI negativeText;
        
        [Inject] private DataController dataController;
        
        public void Awake()
        {
            canvasGroup = this.GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            canvasGroup.interactable = true;
            canvasGroup.DOFade(1, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                SetupPanel();
            });
        }

        private void SetupPanel()
        {
            dayText.text = $"Day - {dataController.DayIndex + 1}";
            positiveText.text = dataController.GetPositiveCount(dataController.DayIndex).ToString();
            negativeText.text = dataController.GetNegativeCount(dataController.DayIndex).ToString();
        }

        public void OnNextDayClicked()
        {
            dataController.DayIndex++;
        }
        
    }
}