using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace _Game.Scripts
{
    public class GameOverPanelView : MonoBehaviour
    {
        public Image image;
        public Color darknessStart;
        public Color darknessEnd;
        public TextMeshProUGUI scoreText;
        public GameObject container;
        
        [Inject] private DataController _dataController;
        
        public void SetGameOverDarkness(float value)
        {
            var color = Color.Lerp(darknessStart, darknessEnd, value);
            image.DOColor(color, .1f).SetEase(Ease.InOutBounce);
        }

        [Button]
        public void Show()
        {
            scoreText.text = $"{_dataController.GetTotalPositiveCount()}-{_dataController.GetTotalNegativeCount()}";
            SetGameOverDarkness(1);
            container.gameObject.SetActive(true);
            _dataController.ClearData();
        }

        public void PlayButton()
        {
            SceneManager.LoadScene(0);
        }
        
    }
}