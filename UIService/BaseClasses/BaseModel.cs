using System;
using System.Runtime.CompilerServices;
using ECGaming.UISystem.Interfaces;

namespace ECGaming.UISystem.BaseClasses
{
    public class BaseModel: IModel
    {
        public event Action<string, object> OnPropertyChanged = delegate { };
        public virtual void LoudDataUpdate(string propertyName, object value)
        {
            PropertyChanged(value, propertyName);
        }

        public virtual void SilentDataUpdate(string propertyName, object value)
        {
            
        }

        public virtual void ForceUpdate()
        {
        }

        protected void PropertyChanged(object value, [CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged.Invoke(propertyName, value);
        }
    }
}