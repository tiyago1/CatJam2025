using _Game.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts
{
    public enum CatState
    {
        Default,
        Angry,
        Happy,
        Walk,
        Hold
    }

    public class CatView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private SpriteRenderer happyRenderer;
        [SerializeField] private GameObject happyAnimation;
        [SerializeField] private SpriteRenderer holdRenderer;
        [SerializeField] private GameObject angryObject;
        [SerializeField] private Animator walkAnimator;
        [SerializeField] private SpriteRenderer walkSpriteRenderer;
        
        private CatData _data;

        public void Initialize(CatData catData)
        {
            _data = catData;
            ChangeState(CatState.Default);
        }

        [Button]
        public void ChangeState(CatState state)
        {
            angryObject.SetActive(state == CatState.Angry);
            walkAnimator.gameObject.SetActive(false);
            renderer.gameObject.SetActive(false);
            holdRenderer.gameObject.SetActive(false);
            happyRenderer.gameObject.SetActive(false);
            happyAnimation.SetActive(false);

            switch (state)
            {
                case CatState.Default:
                    renderer.gameObject.SetActive(true);
                    renderer.sprite = _data.Idle;
                    break;
                case CatState.Angry:
                    renderer.gameObject.SetActive(true);
                    break;
                case CatState.Happy:
                    happyRenderer.gameObject.SetActive(true);
                    happyRenderer.sprite = _data.Happy;
                    happyAnimation.SetActive(true);
                    break;
                case CatState.Walk:
                    walkAnimator.gameObject.SetActive(true);
                    walkAnimator.runtimeAnimatorController = _data.Walking;
                    break;
                case CatState.Hold:
                    holdRenderer.gameObject.SetActive(true);
                    holdRenderer.sprite = _data.Hold;
                    break;
            }
        }

        public void Flip(bool isRight)
        {
            walkSpriteRenderer.flipX = isRight;
        }
    }
}