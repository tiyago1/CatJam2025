using System;
using System.Collections.Generic;
using _Game.Enums;
using Cysharp.Threading.Tasks;
using GamePlay.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class Cat : PathfActor, IDisposable
    {
        [SerializeField] private List<BaseRequest> requests;
        [SerializeField] private Transform requestContainer;
        [SerializeField] public CatView view;
        [SerializeField] private RandomPathAI randomPathAI;
        [SerializeField] private AudioSource source;
        
        public AreaType AreaType;
        public BaseRequest Request;
        public CatState State;

        [Inject] private DiContainer _container;
        [Inject] private DataController _dataController;
        [Inject] private SoundContoller _soundContoller;

        public void Initialize()
        {
            view.Initialize(_dataController.GetRandomCat());
            GoRandomPath();
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
            _soundContoller.PlaySoundVFX(source,SoundType.Success);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1),
                cancellationToken: destroyCancellationToken);
            GoRandomPath();
        }

        public async UniTask Decline()
        {
            ChangeState(CatState.Angry);
            ResetRequest();
            _soundContoller.PlaySoundVFX(source,SoundType.Fail);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1),
                cancellationToken: destroyCancellationToken);
            GoRandomPath();
        }

        public async UniTask NextRequest()
        {
            ChangeState(CatState.Default);

            await UniTask.Delay(TimeSpan.FromSeconds(_dataController.Day.GetRequestWaitDuration()),
                cancellationToken: destroyCancellationToken);
            SetRandomRequest();
        }

        public void SetRandomRequest()
        {
            var request = Random.Range(0, requests.Count);
            request = _dataController.Day.CatTypes[request];
            
            SetRequestType((RequestType)request);
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
            randomPathAI.Activate();
            ChangeState(CatState.Walk);
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

        public void Dispose()
        {
            randomPathAI?.Dispose();
            ResetRequest();
        }
        
    }
}