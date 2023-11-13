using Cinemachine;
using UnityEngine;

namespace InternalAssets.Scripts.Infrastructure.Services.UISystem
{
    public class UIRootView: MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemaCamera;
        private Camera _mainCamera;

        [SerializeField] private Canvas rootCanvas;
        
        public RectTransform CoverLayer;
        public RectTransform MainLayer;
        public RectTransform NotificationLayer;
        public RectTransform VFXLayer;
        public CinemachineVirtualCamera CinemaCamera => _cinemaCamera;
        public Camera MainCamera => _mainCamera;

        public void SetupCameraFOV()
        {
            
        }

        public void SetCameras(Camera camera, CinemachineVirtualCamera virtualCamera)
        {
            _mainCamera = camera;
            _cinemaCamera = virtualCamera;
            rootCanvas.worldCamera = camera;
        }
    }
}