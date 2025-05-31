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


        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GoRandomPath();
            }
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
            SetRequestType(RequestType.Food);
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
            randomPathAI. Activate();
        }

        public override void OnTargetReached()
        {
            base.OnTargetReached();
            Debug.Log("OnTargetReached");
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