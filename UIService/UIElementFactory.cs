using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ECGaming.UISystem.BaseClasses;
using ECGaming.UISystem.Interfaces;
using InternalAssets.Scripts.Infrastructure.AssetManagement;
using InternalAssets.Scripts.Infrastructure.Services.CameraService;
//using InternalAssets.Scripts.Infrastructure.Services.ConfigData;
//using InternalAssets.Scripts.Infrastructure.Services.TutorialService;
using InternalAssets.Scripts.Infrastructure.Services.UISystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Object = UnityEngine.Object;

namespace ECGaming.UISystem
{
    public class UIElementFactory
    {
        private const string UI_Root = "UIRoot";


        private readonly IInstantiator _instantiator;
        private readonly IAssetsProvider _assetsProvider;
        private readonly UIRootView _uiRootViewPrefab;
        //private readonly IConfigDataService dataService;
        //private readonly ITutorialService tutorialService;
        private readonly ICameraService _cameraService;
        private readonly Dictionary<UIElements, IView> _views = new Dictionary<UIElements, IView>();
        private UIRootView _uiRoot;
        private int _sortingOrder;

        public Dictionary<UIElements, IView> LoadedViews => _views;

        public UIElementFactory(IInstantiator instantiator, IAssetsProvider assetsProvider,
            UIRootView uiRootViewPrefab, ICameraService cameraService)
        {
            _instantiator = instantiator;
            _assetsProvider = assetsProvider;
            _uiRootViewPrefab = uiRootViewPrefab;
            _cameraService = cameraService;
            //this.dataService = dataService;
            //this.tutorialService = tutorialService;
        }

        /// <summary>
        /// Load all view from addressables
        /// </summary>
        /// <param name="uiStateConfig"></param>
        public async UniTask Preload(UIStateConfig uiStateConfig)
        {
            foreach (var uiElementReference in uiStateConfig.UIElementReferences)
            {
                if(_views.ContainsKey(uiElementReference.UIElement))
                    continue;
                
                var view = await LoadView(uiElementReference.UIReferenced);
                //if(view is ITutorialTarget tutorialTarget) tutorialService.AddTutorialTarget(tutorialTarget);
                _views.Add(uiElementReference.UIElement, view);
            }
        }

        public UniTaskVoid ReleaseView(UIElements uiElement)
        {
            var view = _views[uiElement];
            view.Destroy();
            _views.Remove(uiElement);
            return default;
        }

        /// <summary>
        /// Loads only tutorial element prefab
        /// </summary>
        public async UniTask LoadTutorialUI()
        {
            //var tutorialReference = dataService.GetUIConfigData().TutorialUIElementReference;
            //var view = await LoadView(tutorialReference.UIReferenced);
            //views.Add(tutorialReference.UIElement, view);
        }

        /// <summary>
        /// Create UIRoot and return UIRootView
        /// </summary>
        /// <returns></returns>
        public async UniTask<UIRootView> CreateUIRoot()
        {
            _uiRoot = Object.Instantiate(_uiRootViewPrefab);
            _uiRoot.name = UI_Root;
            var camera = await _cameraService.GetCamera();
            var vCamera = await _cameraService.GetVirtualCamera();
            _uiRoot.SetCameras(camera, vCamera);
            Object.DontDestroyOnLoad(_uiRoot);
            return _uiRoot;
        }

        /// <summary>
        /// Asynchronously loading and instantiate view prefab and links it to controller
        /// </summary>
        /// <param name="uiElementReference"></param>
        /// <param name="controller"></param>
        private async UniTask<IView> LoadView(AssetReferenceGameObject uiElementReference)
        {
            var prefab = await _assetsProvider.LoadAsync<GameObject>(uiElementReference);
            var instance = _instantiator.InstantiatePrefab(prefab, _uiRoot.transform);
            instance.SetActive(false);
            var baseView = instance.GetComponent<BaseView>();
            SetSortingOrder(baseView);
            SetParentLayer(baseView);
            return baseView;
        }

        /// <summary>
        /// Set parent by parent layer type field in BaseView
        /// </summary>
        /// <param name="baseView"></param>
        private void SetParentLayer(BaseView baseView)
        {
            switch (baseView.ParentLayer)
            {
                case ParentLayer.Cover:
                    baseView.transform.SetParent(_uiRoot.CoverLayer);
                    break;
                case ParentLayer.Main:
                    baseView.transform.SetParent(_uiRoot.MainLayer);
                    break;
                case ParentLayer.Notification:
                    baseView.transform.SetParent(_uiRoot.NotificationLayer);
                    break;
                case ParentLayer.VFX:
                    baseView.transform.SetParent(_uiRoot.VFXLayer);
                    break;
            }
        }

        private void SetSortingOrder(BaseView baseView)
        {
            var canvas = baseView.GetComponent<Canvas>();
            canvas.sortingOrder = _sortingOrder;
            _sortingOrder++;
        }
    }
}