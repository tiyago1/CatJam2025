using System;
using System.Data.Common;
using _Game.Enums;
using _Game.Signals;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class AreaRequest : BaseRequest, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private AreaRequestView view => (AreaRequestView)requestView;
        [SerializeField] private Vector2 boxSize;

        [Inject(Id = "Garden")] private BoxCollider2D garden;
        [Inject(Id = "Main")] private BoxCollider2D main;
        [Inject(Id = "Dark")] private BoxCollider2D dark;

        public AreaType AreaType;
        private bool _isDragging;
        private RaycastHit2D[] _hits;
        private Vector2 position;

        [Button]
        public override void Initialize(Cat cat)
        {
            AreaType = (AreaType)Random.Range(0, 3);
            _hits = new RaycastHit2D[10];
            view.Initialize(AreaType);
            
            base.Initialize(cat);
        }

        public override void Solve()
        {
            if (IsRequestIgnored)
                return;
            
            CancelTimer();
            if (Cat.AreaType == AreaType)
            {
                Cat.Approved().Forget();
                SignalBus.Fire<GameSignals.OnSuccessRequest>();
                
            }
            else
            {
                Cat.Decline().Forget();
                SignalBus.Fire<GameSignals.OnFailRequest>();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsValid() || IsRequestIgnored)
                return;

            _isDragging = true;
        }

        private void Update()
        {
            if (!_isDragging)
                return;

            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = 0;

            Vector2 entity = this.transform.position;
            var x = Physics2D.OverlapBox(entity, boxSize, 0, LayerMask.GetMask("Area"));
            if (x != null)
            {
                position = point;
            }

            Cat.transform.position = point;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Vector3 pos = Vector3.zero;
            if (Cat.AreaType != AreaType.Outside)
            {
                switch (GetClosetAreaType())
                {
                    case AreaType.Dark:
                        pos = dark.ClosestPoint(this.transform.position);
                        break;
                    case AreaType.Main:
                        pos = main.ClosestPoint(this.transform.position);
                        break;
                    case AreaType.Garden:
                        pos = garden.ClosestPoint(this.transform.position);
                        break;
                }
            }
            else
            {
                pos = position;
            }
      
            _isDragging = false;
            Cat.transform.DOMove(pos, .1f);
            Solve();
        }

        private AreaType GetClosetAreaType()
        {
            float darkDistance = 0;
            float mainDistance = 0;
            float gardenDistance = 0;

            darkDistance = (this.transform.position - dark.transform.position).magnitude;
            mainDistance = (this.transform.position - main.transform.position).magnitude;
            gardenDistance = (this.transform.position - garden.transform.position).magnitude;

            if (darkDistance < mainDistance && darkDistance < gardenDistance)
            {
                return AreaType.Dark;
            }

            if (mainDistance < gardenDistance && mainDistance < darkDistance)
            {
                return AreaType.Main;
            }

            if (gardenDistance < mainDistance && gardenDistance < darkDistance)
            {
                return AreaType.Garden;
            }

            return AreaType.Garden;
        }
    }
}