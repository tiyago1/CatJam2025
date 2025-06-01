using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Game.Scripts
{
    public class LastDayFinishedPanelView : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public TextMeshProUGUI scoreText;
        
        [Inject] private DataController dataController;
        [Inject] private SoundContoller soundContoller;
        
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
            scoreText.text = dataController.GetTotalPositiveCount().ToString();
            scoreText.text += "-";
            scoreText.text += dataController.GetTotalNegativeCount().ToString();
        }

        public void OnFinish()
        {
            dataController.ClearData();
            soundContoller.PlayClickSound();
            SceneManager.LoadScene(1);
        }
        
    }
}