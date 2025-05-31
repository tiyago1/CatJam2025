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
        
        [SerializeField] private float minRotation;
        [SerializeField] private float maxRotation;

        [Inject] private DataController _dataController; 
        
        public void Initialize()
        {
            SetDayIndicator(1);
            dayText.text = $"Day - {_dataController.DayIndex + 1}";
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
        }
        
    }
}