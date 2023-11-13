using ECGaming.UISystem.BaseClasses;
using Zenject;

namespace InternalAssets.Scripts.UISystem.Example
{
    public class ExampleUIController: BaseController
    {
        public ExampleUIController(DiContainer container)
        {
            model = new ExampleUIModel();   
        }
    }
}