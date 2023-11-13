using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ECGaming.UISystem.BaseClasses;
using ECGaming.UISystem.Interfaces;
using InternalAssets.Scripts.Infrastructure.Services.UISystem;
using UnityEngine;

namespace ECGaming.UISystem
{
    public class UIDirector
    {
        private readonly UIElementFactory _elementFactory;
        private readonly UIControllerFactory _uiControllerFactory;

        private readonly Dictionary<UIElements, IController> _uiControllers = new Dictionary<UIElements, IController>();
        private readonly Dictionary<UIElements, IController> _controllersToInitialize = new Dictionary<UIElements, IController>();
        

        public UIDirector(UIElementFactory uiElementFactory, UIControllerFactory uiControllerFactory)
        {
            _elementFactory = uiElementFactory;
            _uiControllerFactory = uiControllerFactory;
        }

        /// <summary>
        /// Dispose controller and removes it from collections
        /// </summary>
        /// <param name="uiElement"></param>
        public void ReleaseController(UIElements uiElement)
        {
            
            if (_controllersToInitialize.ContainsKey(uiElement))
            {
                var controller = _controllersToInitialize[uiElement];
                controller.Dispose();
                _controllersToInitialize.Remove(uiElement);
            }
            if(_uiControllers.ContainsKey(uiElement))
            {
                var controller = _uiControllers[uiElement];
                controller.Dispose();
                _uiControllers.Remove(uiElement);
            }
        }

        /// <summary>
        /// Create new ui element if it not added at uiControllers and set active state for already added,
        /// Called InitializeControllers function to initialize controllers
        /// </summary>
        /// <typeparam name="T">controller type inherited from IController or BaseController</typeparam>
        /// <returns></returns>
        public async UniTask AddUIElement(UIElements uiElement, BaseController controller)
        {
            if (CheckControllerDictionary(uiElement))
                _uiControllers[uiElement].Show();
            else
                await CreateUIElement(uiElement, controller);
        }

        /// <summary>
        /// Update UI element model by type of controller
        /// </summary>
        /// <param name="propertyName">name of the property to update</param>
        /// <param name="value">new property value</param>
        /// <typeparam name="T">controller type inherited from IController or BaseController</typeparam>
        /// <returns></returns>
        public UIDirector UpdateUIElementModel<T>(string propertyName, object value, bool useSilentUpdate = false) where T: IController
        {
            foreach (var pair in _uiControllers)
            {
                if (pair.Value is T)
                {
                    pair.Value.UpdateModel(propertyName, value, useSilentUpdate);
                    break;
                }
            }
            return this;
        }

        /// <summary>
        /// Updates only view fields without changing model
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public UIDirector UpdateUIElementView<T>(string propertyName, object value) where T : IController
        {
            foreach (var pair in _uiControllers)
            {
                if (pair.Value is T)
                {
                    pair.Value.UpdateOnlyView(propertyName, value);
                    break;
                }
            }
            return this;
        }

        /// <summary>
        /// Change state of the UI element game object to active by controller type
        /// </summary>
        /// <typeparam name="T">controller type inherited from IController or BaseController</typeparam>
        /// <returns></returns>
        public UIDirector ShowUIElement<T>() where T: IController
        {
            foreach (var pair in _uiControllers)
            {
                if (pair.Value is T)
                {
                    pair.Value.Show();
                    break;
                }
            }

            return this;
        }

        /// <summary>
        /// Change state of the UI element game object to active by name
        /// </summary>
        /// <param name="uiElementName">UI element name</param>
        /// <returns></returns>
        public UIDirector ShowUIElement(UIElements uiElementName)
        {
            if(!_uiControllers.ContainsKey(uiElementName))
            {
                Debug.Log($"{uiElementName} is not in this state");
                return this;
            }
            _uiControllers[uiElementName].Show();
            return this;
        }


        /// <summary>
        /// Change state of the UI element game object to inactive by controller type
        /// </summary>
        /// <typeparam name="T">controller type inherited from IController or BaseController</typeparam>
        /// <returns></returns>
        public UIDirector HideUIElement<T>() where T: IController
        {
            foreach (var pair in _uiControllers)
            {
                if (pair.Value is T)
                {
                    pair.Value.Hide();
                    break;
                }
            }

            return this;
        }

        /// <summary>
        /// Change state of the UI element game object to inactive by name
        /// </summary>
        /// <param name="uiElementName">UI element name</param>
        /// <returns></returns>
        public UIDirector HideUIElement(UIElements uiElementName)
        {
            _uiControllers[uiElementName].Hide();
            return this;
        }

        /// <summary>
        /// Provides IController of required type
        /// </summary>
        /// <typeparam name="T">controller type inherited from IController or BaseController</typeparam>
        /// <returns></returns>
        public T GetController<T>() where T : IController
        {
            foreach (var pair in _uiControllers)
            {
                if (pair.Value is T)
                {
                    return (T) pair.Value;
                }
            }
            return default;
        }

        public List<IController> GetAllControllers()
            => _uiControllers.Values.ToList();

        
        /// <summary>
        /// Получить enum по типу IController
        /// </summary>
        public UIElements GetTypeByController<T>() where T : IController
        {
            foreach (var pair in _uiControllers)
            {
                if (pair.Value is T)
                {
                    return pair.Key;
                }
            }

            return default;
        }

        private async UniTask CreateUIElement(UIElements uiElement, IController controller)
        {
            controller = await _uiControllerFactory.CreateController(uiElement);
            controller.Setup(_elementFactory.LoadedViews[uiElement]);
            _uiControllers.Add(uiElement, controller);
            _controllersToInitialize.Add(uiElement, controller);
        }


        private bool CheckControllerDictionary<T>() where T : IController
        {
            foreach (var pair in _uiControllers)
            {
                if (pair.Value is T)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckControllerDictionary(UIElements uiElements)
        {
            return _uiControllers.ContainsKey(uiElements);
        }

        /// <summary>
        /// Set inactive all controllers except T
        /// </summary>
        /// <typeparam name="T">controller which stay active or become active, inheritor of IController or BaseController</typeparam>
        public void HideAllUIElementsExcept<T>() where T : IController
        {
            foreach (var pair in _uiControllers)
            {
                if (pair.Value is T)
                {
                    pair.Value.Show();
                }
                else
                {
                    pair.Value.Hide();
                }
            }
        }
    }
}