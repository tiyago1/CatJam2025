using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Game.Scripts
{
    public class DayFinishedPanelView : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public TextMeshProUGUI dayText;
        public TextMeshProUGUI scoreText;
        
        [Inject] private DataController dataController;
        
        public void Awake()
        {
            canvasGroup = this.GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            SetupPanel();
            canvasGroup.DOFade(1, 1f).SetEase(Ease.Linear);
        }

        private void SetupPanel()
        {
            dayText.text = $"DAY - {dataController.DayIndex + 1} COMPLETED";
            scoreText.text = dataController.GetPositiveCount(dataController.DayIndex).ToString();
            scoreText.text += "-";
            scoreText.text += dataController.GetNegativeCount(dataController.DayIndex).ToString();
        }

        public void OnNextDayClicked()
        {
            dataController.DayIndex++;
            SceneManager.LoadScene(1);
        }
        
    }
}