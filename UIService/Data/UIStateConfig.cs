using System.Collections.Generic;
using ECGaming.UISystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace InternalAssets.Scripts.Infrastructure.Services.UISystem
{
    [CreateAssetMenu(fileName = "StateUIConfig", menuName = "Configs/StateUIConfig")]
    public class UIStateConfig: ScriptableObject
    {
        public UIStateConfigType UIStateConfigType;
        public List<UIElementReference> UIElementReferences;
        
        public AssetReferenceGameObject GetUIElementReference(UIElements uiElement)
        {
            foreach (var uiElementReference in UIElementReferences)
            {
                if (uiElementReference.UIElement.Equals(uiElement))
                {
                    return uiElementReference.UIReferenced;
                }
            }

            return null;
        }

        public void OnValidate()
        {
            foreach (var uiElementReference in UIElementReferences)
            {
                var elementReference = uiElementReference;
                elementReference.UIElementName = elementReference.UIElement.ToString();
            }
        }
    }
}