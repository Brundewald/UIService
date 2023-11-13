using System;
using ECGaming.UISystem.BaseClasses;
using ECGaming.UISystem.Interfaces;

namespace InternalAssets.Scripts.UISystem.Example
{
    public class ExampleUIModel: BaseModel
    {
        private int count;
        
        public int Count
        {
            get => count;
            set
            {
                if (value != count)
                {
                    count = value;
                    PropertyChanged(count);
                }
            }
        }

        public static string CountName => nameof(Count);

        public void SetInitialData(IModelInitialData initialData, bool forceUpdate)
        {
            var data = initialData as ExampleData;
            count = data.Count;
        }

        public void LoudDataUpdate(string propertyName, object value)
        {
            if (propertyName.Equals(nameof(Count)))
            {
                Count = (int) value;
            }
        }

        public void SilentDataUpdate(string propertyName, object value)
        {
            if (propertyName.Equals(nameof(Count)))
            {
                count = (int) value;
            }
        }

        public void ForceUpdate()
        {
            PropertyChanged(count, nameof(Count));
        }
    }
}