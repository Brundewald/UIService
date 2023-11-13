using ECGaming.UISystem.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ECGaming.UISystem.BaseClasses
{
    [RequireComponent(typeof(Canvas),typeof(GraphicRaycaster))]
    public abstract class BaseView: MonoBehaviour, IView
    {
        public Canvas Canvas { get; private set; }
        public ParentLayer ParentLayer;
        public event Action<ViewActionKey, object> OnViewEventTriggered = delegate {  };
        public event Action OnViewClosed = delegate {  };
        
        public virtual void UpdateView(string propertyName, object value)
        {
            
        }
        
        protected void InvokeViewEvent(ViewActionKey viewActionKey, object value)
        {
            OnViewEventTriggered.Invoke(viewActionKey, value);
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnViewClosed.Invoke();
        }

        public void SetRenderingOrder(int sortingOrder)
        {
            Canvas.overrideSorting = true;
            Canvas.sortingOrder = sortingOrder;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        protected virtual void Start()
        {
            Canvas = GetComponent<Canvas>();
        }
    }

    public enum ParentLayer
    {
        None,
        Cover,
        Main,
        Notification,
        VFX
    }
}