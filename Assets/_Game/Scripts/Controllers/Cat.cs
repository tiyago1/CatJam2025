using System;
using System.Collections.Generic;
using _Game.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _Game.Scripts
{
    public class Cat : MonoBehaviour
    {
        [SerializeField] private List<BaseRequest> requests;
        [SerializeField] private Transform requestContainer;

        public AreaType AreaType;
        public BaseRequest Request;

        [Inject] private DiContainer _container;
        
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

        public void Initialize()
        {
        }

        [Button]
        public void SetRequestType(RequestType requestType)
        {
            ResetRequest();
            Request = _container.InstantiatePrefabForComponent<BaseRequest>(requests[(int)requestType], requestContainer);
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
    }
}