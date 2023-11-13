using System;
using ECGaming.UISystem.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ECGaming.UISystem
{
    [Serializable]
    public struct UIElementReference
    {
        [HideInInspector] public string UIElementName;
        public UIElements UIElement;
        public BaseController Controller; 
        public AssetReferenceGameObject UIReferenced;
    }
}
    