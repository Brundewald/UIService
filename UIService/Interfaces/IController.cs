using System;
using ECGaming.UISystem.BaseClasses;

namespace ECGaming.UISystem.Interfaces
{
    public interface IController
    {
        /// <summary>
        /// Cash local instance of the IView and provide subscription IView to IModel  
        /// </summary>
        /// <param name="viewInstance">inheritor of IView or BaseView</param>
        void Setup(IView viewInstance);


        IController Initialize(InitializationInfo initializationInfo);
        
        /// <summary>
        /// Change state of IView game object to active
        /// </summary>
        IController Show();

        /// <summary>
        /// Change state of IView game object to inactive
        /// </summary>
        IController Hide();

        /// <summary>
        /// Unsubscribe IView from IModel and removes all IView listeners 
        /// </summary>
        void Dispose();

        /// <summary>
        /// Update model data property by property name 
        /// </summary>
        /// <param name="propertyName">name of the property to update</param>
        /// <param name="value">updated value</param>
        /// <param name="useSilentUpdate">Update without view update</param>
        void UpdateModel(string propertyName, object value, bool useSilentUpdate = false);

        void UpdateOnlyView(string propertyName, object value);
    }
}