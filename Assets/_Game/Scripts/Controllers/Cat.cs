using System;
using System.Collections.Generic;
using System.Diagnostics;
using _Game.Enums;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class Cat : MonoBehaviour
    {
        [SerializeField] private List<BaseRequest> requests;
        [SerializeField] private Transform requestContainer;
        [SerializeField] public CatView view;

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
            NextRequest().Forget();
        }

        public async UniTask Decline()
        {
            ChangeState(CatState.Angry);
            ResetRequest();

            await UniTask.Delay(TimeSpan.FromSeconds(1));
            NextRequest().Forget();
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

        public void OnTimeCompleted()
        {
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