using System;

namespace ECGaming.UISystem.Interfaces
{
    public interface IModel
    {
        /// <summary>
        /// Invokes on every property value update
        /// </summary>
        event Action<string, object> OnPropertyChanged;
        
        /// <summary>
        /// Update model property and with View notification via OnPropertyChange event.
        /// </summary>
        /// <param name="propertyName">name of the property to update</param>
        /// <param name="value">updated value</param>
        void LoudDataUpdate(string propertyName, object value);

        /// <summary>
        /// Update model property and without notify View.
        /// </summary>
        /// <param name="propertyName">name of the property to update</param>
        /// <param name="value">updated value</param>
        void SilentDataUpdate(string propertyName, object value);
        
        /// <summary>
        /// Use to force update view by invoke OnPropertyChanged
        /// </summary>
        void ForceUpdate();
    }
}