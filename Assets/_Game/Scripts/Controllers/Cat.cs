using System;
using System.Collections.Generic;
using _Game.Enums;
using Cysharp.Threading.Tasks;
using GamePlay.Components;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace _Game.Scripts
{
    public class Cat : PathfActor
    {
        [SerializeField] private List<BaseRequest> requests;
        [SerializeField] private Transform requestContainer;
        [SerializeField] public CatView view;
        [SerializeField] private RandomPathAI randomPathAI;

        public AreaType AreaType;
        public BaseRequest Request;
        public CatState State;

        [Inject] private DiContainer _container;
        [Inject] private DataController _dataController;


        public void Initialize()
        {
            SetRandomRequest();

            view.Initialize(_dataController.GetRandomCat());
        }

        protected override void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition,
            out Quaternion nextRotation)
        {
            base.MovementUpdateInternal(deltaTime, out nextPosition, out nextRotation);
            view.Flip(nextPosition.x > transform.position.x);
        }

    
        public void ChangeState(CatState state)
        {
            State = state;
            view.ChangeState(State);
        }

        public async UniTask Approved()
        {
            ChangeState(CatState.Happy);
            ResetRequest();

            await UniTask.Delay(TimeSpan.FromSeconds(1));
            GoRandomPath();
        }

        public async UniTask Decline()
        {
            ChangeState(CatState.Angry);
            ResetRequest();

            await UniTask.Delay(TimeSpan.FromSeconds(1));
            GoRandomPath();
        }

        public async UniTask NextRequest()
        {
            ChangeState(CatState.Default);
            await UniTask.Delay(TimeSpan.FromSeconds(_dataController.Day.GetRequestWaitDuration()));
            SetRandomRequest();
        }

        public void SetRandomRequest()
        {
            SetRequestType(RequestType.Area);
        }

        [Button]
        public void SetRequestType(RequestType requestType)
        {
            ResetRequest();
            Request = _container.InstantiatePrefabForComponent<BaseRequest>(requests[(int)requestType],
                requestContainer);
            Request.Initialize(this);
        }

        public void ResetRequest()
        {
            if (Request == null)
                return;

            Destroy(Request.gameObject);
            Request = null;
        }

        [Button]
        public void GoRandomPath()
        {
            WaitAndLook().Forget();
        }

        private async UniTask WaitAndLook()
        {
            var previousPosX = randomPathAI.Activate().x;
            ChangeState(CatState.Walk);
            await UniTask.Delay(TimeSpan.FromSeconds(.1f));
            // view.Flip(previousPosX < this.transform.position.x);
        }

        public override void OnTargetReached()
        {
            base.OnTargetReached();
            NextRequest().Forget();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Dark"))
            {
                AreaType = AreaType.Dark;
            }

            if (other.CompareTag("Main"))
            {
                AreaType = AreaType.Main;
            }

            if (other.CompareTag("Garden"))
            {
                AreaType = AreaType.Garden;
            }

            if (other.CompareTag("Outside"))
            {
                AreaType = AreaType.Outside;
            }
        }
    }
}