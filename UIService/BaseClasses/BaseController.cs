using System;
using System.Collections.Generic;
using ECGaming.UISystem.Interfaces;

namespace ECGaming.UISystem.BaseClasses
{
    public abstract class BaseController : IController
    {
        private readonly List<Action<ViewActionKey, object>> viewListeners = new List<Action<ViewActionKey, object>>();
        protected IModel model;
        protected IView view;
        private bool viewIsActive;
        
        public event Action OnControllerShowed = delegate {  };
        public event Action OnControllerHided = delegate {  };

        public virtual void Setup(IView viewInstance)
        {
            view = viewInstance;
            view.SetActive(viewIsActive);
            view.OnViewEventTriggered += HandleViewClosed;
            if (viewListeners.Count > 0) SubscribeListeners();
            if (model == null) return;
            model.OnPropertyChanged += view.UpdateView;
        }

        public virtual IController Initialize(InitializationInfo initializationInfo)
        {
            return this;
        }

        public virtual IController Show()
        {
            if (viewIsActive) return this;

            viewIsActive = true;
            model?.ForceUpdate();
            view?.Show();
            
            OnControllerShowed.Invoke();
            return this;
        }

        public virtual IController Hide()
        {
            view?.Hide();
            viewIsActive = false;

            OnControllerHided.Invoke();
            return this;
        }

        public void UpdateModel(string propertyName, object value, bool useSilentUpdate = false)
        {
            if(useSilentUpdate)
                model.SilentDataUpdate(propertyName, value);
            else
                model.LoudDataUpdate(propertyName, value);
        }

        public virtual void UpdateOnlyView(string propertyName, object value)
        {
            view.UpdateView(propertyName, value);
        }

        public virtual void Dispose()
        {
            model.OnPropertyChanged -= view.UpdateView;
            view.Destroy();
        }

        private void SubscribeListeners()
        {
            foreach (var listener in viewListeners)
            {
                view.OnViewEventTriggered += listener;
            }
        }

        private void HandleViewClosed(ViewActionKey arg1, object arg2)
        {
            viewIsActive = false;
        }
    }
}