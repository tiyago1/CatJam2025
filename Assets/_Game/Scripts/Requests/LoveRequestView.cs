using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class LoveRequestView : RequestView
    {
        [SerializeField] private Image fillImage;

        public void Initialize()
        {
            fillImage.transform.localScale = Vector3.zero;
        }

        [Button]
        public void FillAmountValue(float amount)
        {
            Vector3 target = Vector3.Lerp(Vector3.zero, Vector3.one, amount);
            fillImage.transform.localScale = target;
            // fillImage.transform.DOScale(target, .1f).SetEase(Ease.InOutElastic).OnComplete(callback.Invoke);
            fillImage.transform.parent.transform.DOShakeRotation(.3f, 30, 5, 15, true, ShakeRandomnessMode.Harmonic);
        }
    }
}