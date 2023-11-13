using System;

namespace ECGaming.UISystem.Interfaces 
{
    public interface IView
    {
        /// <summary>
        /// Proxy event for user input actions.
        /// Sends event key and result of user input.
        /// </summary>
        public event Action<ViewActionKey, object> OnViewEventTriggered;
        
        /// <summary>
        /// Updates view when model changes
        /// </summary>
        /// <param name="propertyName">name of the property to update</param>
        /// <param name="value">updated value</param>
        public void UpdateView(string propertyName, object value);
        
        /// <summary>
        /// Change state of view go
        /// </summary>
        /// <param name="state"></param>
        public void SetActive(bool state);

        /// <summary>
        /// Set view canvas sorting order
        /// </summary>
        /// <param name="sortingOrder"></param>
        public void SetRenderingOrder(int sortingOrder);
        
        /// <summary>
        /// Destroy view go
        /// </summary>
        public void Destroy();

        void Show();
        void Hide();
    }
}
