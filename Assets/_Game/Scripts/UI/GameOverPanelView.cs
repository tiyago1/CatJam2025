using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class GameOverPanelView : MonoBehaviour
    {
        public Image image;
        public Color darknessStart;
        public Color darknessEnd;
        
        public void SetGameOverDarkness(float value)
        {
            var color = Color.Lerp(darknessStart, darknessEnd, value);
            image.DOColor(color, .1f).SetEase(Ease.InOutBounce);
        }
    }
}