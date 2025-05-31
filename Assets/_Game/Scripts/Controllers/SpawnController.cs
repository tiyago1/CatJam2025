using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Game.Scripts
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] private List<Transform> points;
        [SerializeField] private Cat catPrefab;

        [Inject] private DiContainer _container;
        [Inject] private DataController _dataController;
        
        [Button]
        public void Spawn()
        {
            var point = points[Random.Range(0, points.Count)];
            var cat = _container.InstantiatePrefabForComponent<Cat>(catPrefab, point.transform.position,
                Quaternion.identity,
                this.transform);
            
            cat.Initialize();
        }

        public async UniTask Initialize()
        {
            points = this.GetComponentsInChildren<Transform>().ToList();
            
            for (int i = 0; i < _dataController.Day.CatCount; i++)
            {
                Spawn();
                await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            }
        }
    }
}