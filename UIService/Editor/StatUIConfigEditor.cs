using System;
using InternalAssets.Scripts.Infrastructure.Services.UISystem;
using UnityEditor;

namespace InternalAssets.Scripts.Infrastructure.UI.Editor
{
    [CustomEditor(typeof(UIStateConfig))]
    public class StatUIConfigEditor: UnityEditor.Editor
    {
        private UIStateConfig _target;

        private SerializedObject _controllerType;
        
        private void OnEnable()
        {
            _target = (UIStateConfig) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            
        }
    }
}