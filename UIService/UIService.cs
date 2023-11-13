using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ECGaming.UISystem.Interfaces;
using InternalAssets.Scripts.Infrastructure.AssetManagement;
using InternalAssets.Scripts.Infrastructure.Constants;
using InternalAssets.Scripts.Infrastructure.Services.UISystem;
using ModestTree;

namespace ECGaming.UISystem
{
    public class UIService: IUIService
    {
        private readonly UIElementFactory _uiElementFactory;
        private readonly UIControllerFactory _uiControllerFactory;
        private readonly UIDirector _uiDirector;
        private readonly IAssetsProvider _assetsProvider;
        private readonly Dictionary<UIStateConfigType, UIStateConfig> _loadedStates = new Dictionary<UIStateConfigType, UIStateConfig>();
        private readonly List<UIElements> _showingElements = new();
        private UIRootView _uiRootInstance;
        private bool _uiReady;

        public bool IsReady => _uiRootInstance != null;


        public UIService(UIElementFactory uiElementFactory, UIDirector uiDirector, IAssetsProvider assetsProvider)
        {
            _uiElementFactory = uiElementFactory;
            _uiDirector = uiDirector;
            _assetsProvider = assetsProvider;
        }

        /// <summary>
        /// Проверяет загружен ли данный конфиг или нет
        /// </summary>
        /// <param name="uiStateConfig"></param>
        /// <returns></returns>
        public bool StateIsLoaded(UIStateConfigType uiStateConfig)
        {
            return _loadedStates.ContainsKey(uiStateConfig);
        }

        public async UniTask CreateUIRoot()
        {
            if(!_uiReady)
                _uiRootInstance = await _uiElementFactory.CreateUIRoot();
        }

        public async UniTask Preload(UIStateConfigType configType)
        {
            if (_loadedStates.ContainsKey(configType))
                return;
            
            var uiStateConfig = await _assetsProvider.LoadAsync<UIStateConfig>(GetAddress(configType));
            Assert.IsNotNull(uiStateConfig, $"{uiStateConfig} not loaded check addressable or key");
            
            await _uiElementFactory.Preload(uiStateConfig);
            
            foreach (var uiElementReference in uiStateConfig.UIElementReferences)
            {
                _uiDirector.AddUIElement(uiElementReference.UIElement, uiElementReference.Controller).Forget();
            }
            
            _loadedStates.Add(uiStateConfig.UIStateConfigType, uiStateConfig);
        }

        public UniTask ReleaseState(UIStateConfigType uiState)
        {
            foreach (var uiElementReference in _loadedStates[uiState].UIElementReferences)
            {
                _uiDirector.ReleaseController(uiElementReference.UIElement);
                _uiElementFactory.ReleaseView(uiElementReference.UIElement).Forget();
            }

            _loadedStates.Remove(uiState);
            
            return default;
        }

        public T GetController<T>() where T : IController
        {
            return _uiDirector.GetController<T>();
        }
        
        public List<IController> GetAllControllers()
        {
            return _uiDirector.GetAllControllers();
        }

        public void UpdateUIElementModel<T>(string propertyName, object value, bool useSilentUpdate = false) where T : IController
        {
            _uiDirector.UpdateUIElementModel<T>(propertyName, value, useSilentUpdate);
        }


        public void UpdateUIElementView<T>(string propertyName, object value) where T : IController
        {
            _uiDirector.UpdateUIElementView<T>(propertyName, value);
        }

        public void ShowState(UIStateConfigType uiState)
        {
            if (!_loadedStates.ContainsKey(uiState))
            {
                throw new Exception($"{uiState} is not loaded");
            }

            foreach (var uiElementReference in _loadedStates[uiState].UIElementReferences)
            {
                _uiDirector.ShowUIElement(uiElementReference.UIElement);
                _showingElements.Add(uiElementReference.UIElement);
            }
        }

        public void ShowStateWithMerge(UIStateConfigType uiState)
        {
            if (!_loadedStates.ContainsKey(uiState))
                throw new Exception($"{uiState} is not loaded");

            foreach (var uiElementReference in GetElementsToShow(uiState))
            {
                _uiDirector.ShowUIElement(uiElementReference);
                _showingElements.Add(uiElementReference);
            }
        }

        public void ShowUIElement<T>() where T : IController
        {
            _uiDirector.ShowUIElement<T>();
            _showingElements.Add(_uiDirector.GetTypeByController<T>());
        }

        public void HideState(UIStateConfigType uiState)
        {
            if (!_loadedStates.ContainsKey(uiState))
            {
                throw new Exception($"{uiState} is not loaded");
            }
            
            foreach (var uiElementReference in _loadedStates[uiState].UIElementReferences)
            {
                _uiDirector.HideUIElement(uiElementReference.UIElement);
                _showingElements.Remove(uiElementReference.UIElement);
            }
        }

        public void HideStateWithMerge(UIStateConfigType uiState)
        {
            if (!_loadedStates.ContainsKey(uiState))
                throw new Exception($"{uiState} is not loaded");

            foreach (var uiElementReference in GetElementsToHide(uiState))
            {
                _uiDirector.HideUIElement(uiElementReference);
                _showingElements.Remove(uiElementReference);
            }
        }

        public void HideUIElement<T>() where T : IController
        {
            _uiDirector.HideUIElement<T>();
            _showingElements.Remove(_uiDirector.GetTypeByController<T>());
        }

        private List<UIElements> GetElementsToShow(UIStateConfigType newUIState)
        {
            var elementsToShow = _loadedStates[newUIState].UIElementReferences.Select(x => x.UIElement).ToList();
            foreach (var showingElement in _showingElements)
            {
                if (_loadedStates[newUIState].UIElementReferences.Any(newUIElement => newUIElement.UIElement == showingElement))
                {
                    elementsToShow.Remove(showingElement);
                }
            }
            return elementsToShow;
        }

        private List<UIElements> GetElementsToHide(UIStateConfigType uiState)
        {
            List<UIElements> elementsToHide = new();
            foreach (var showingElement in _showingElements)
            {
                bool isInNewState = _loadedStates[uiState].UIElementReferences.Any(newUIElement => newUIElement.UIElement == showingElement);
                if (!isInNewState)
                    elementsToHide.Add(showingElement);
            }                
            return elementsToHide;
        }

        private string GetAddress(UIStateConfigType configType)
        {
            return configType switch
            {
                UIStateConfigType.InfoPopupState => UIStateConfigNames.InfoPanelConfig,
                UIStateConfigType.MainMenuDefaultState => UIStateConfigNames.MainMenuDefaultConfig,
                UIStateConfigType.MainMenuCharacterState => UIStateConfigNames.MenuCharacterConfig,
                UIStateConfigType.MainMenuGymState => UIStateConfigNames.MenuGymConfig,
                UIStateConfigType.MainMenuWeaponState => UIStateConfigNames.MenuWeaponConfig,
                UIStateConfigType.MainMenuMapState => UIStateConfigNames.MenuMapConfig,
                UIStateConfigType.LoadingState => UIStateConfigNames.LoadingScreenConfig,
                UIStateConfigType.GameplayState => UIStateConfigNames.GameplayConfig,
                UIStateConfigType.InfoMessageState => UIStateConfigNames.InfoMessageConfig,
                UIStateConfigType.MapInterfaceState => UIStateConfigNames.MapInterfaceConfig,
                UIStateConfigType.LobbySettingsState => UIStateConfigNames.LobbySettingsConfig,
                _ => default
            };
        }
    }
}
