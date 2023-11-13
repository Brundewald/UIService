using ECGaming.UISystem;
using InternalAssets.Scripts.GamePlay.Units.UnitPanel;
using InternalAssets.Scripts.Infrastructure.UI.UnitsCanvas;
using InternalAssets.Scripts.Infrastructure.UI.UnitsCanvas.UnitsIndicators;
using MyBox;
using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.Infrastructure.Services.UISystem.Injection
{
    [CreateAssetMenu(fileName = "UiServiceInstaller", menuName = "Installers/UiServiceInstaller")]
    public class UiServiceInstaller : ScriptableObjectInstaller<UiServiceInstaller>
    {
        [SerializeField] private UIRootView uiRootPrefab;
        [DisplayInspector][SerializeField] private UnitCanvasConfig unitPanelConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(uiRootPrefab).IfNotBound();

            Container.BindInterfacesAndSelfTo<UIService>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIElementFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIControllerFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIDirector>().AsSingle();
            
            BindUnitPanels();
            BindUnitIndicator();
        }

        private void BindUnitPanels()
        {
            Container.BindInstance(unitPanelConfig).IfNotBound();
            Container.BindFactory<UnitPanelView, UnitPanelsFactory>()
                .FromComponentInNewPrefab(unitPanelConfig.UnitPanelPrefab);
        }
        
        private void BindUnitIndicator()
        {
            Container.BindFactory<UnitIndicatorView, UnitIndicatorFactory>()
                .FromComponentInNewPrefab(unitPanelConfig.UnitIndicatorViewPrefab);
        }
    }
}