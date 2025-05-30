using System;
using _Game.Signals;
using UnityEngine;
using Zenject;

namespace _Game.Scripts
{
    public class CameraSlideController : MonoBehaviour
    {
        [SerializeField] private float edgeSize = 10f;
        [SerializeField] private float cameraSpeed = 5f;
        [SerializeField] private Camera camera;
        [SerializeField] private float maxX;
        [SerializeField] private float minX;
        [SerializeField] private Vector3 target;
        
        [Inject] private SignalBus _signalBus;
        
        public void Update()
        {
            Vector3 mousePosition = Input.mousePosition;

            if (mousePosition.x <= edgeSize)
            {
                Move(Vector3.left);
                // _signalBus.Fire<GameSignals.OnCameraLeft>();
            }

            if (mousePosition.x >= Screen.width - edgeSize)
            {
                Move(Vector3.right);
                // _signalBus.Fire<GameSignals.OnCameraRight>();
            }
        }

        private void Move(Vector3 direction)
        {
            target = direction * cameraSpeed * Time.deltaTime;
            target += camera.transform.position;
            target.z = -10;
            target.x = Mathf.Clamp(target.x, minX, maxX);
            camera.transform.position = target;
        }
    }
}