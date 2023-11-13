using ECGaming.UISystem;
using ECGaming.UISystem.BaseClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InternalAssets.Scripts.UISystem.Example
{
    public class ExampleUIView: BaseView
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button button;

        private void OnEnable()
        {
            button.onClick.AddListener(ButtonPressed);
        }

        private void OnDisable()
        {
            button.onClick.RemoveAllListeners();
        }

        private void ButtonPressed()
        {
            InvokeViewEvent(ViewActionKey.Exit, null);
        }

        public override void UpdateView(string propertyName, object value)
        {
            if (propertyName.Equals("Count"))
            {
                UpdateText(value);
            }
        }

        private void UpdateText(object value)
        {
            text.text = value.ToString();
        }
    }
}