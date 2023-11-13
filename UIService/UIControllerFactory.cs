using Cysharp.Threading.Tasks;
using ECGaming.UISystem;
using ECGaming.UISystem.Interfaces;
using InternalAssets.Scripts.Infrastructure.UI;
using InternalAssets.Scripts.Infrastructure.UI.ButtonSidePanel;
using InternalAssets.Scripts.Infrastructure.UI.GameplayPanel;
using InternalAssets.Scripts.Infrastructure.UI.InfoMessagePanel;
using InternalAssets.Scripts.Infrastructure.UI.InfoPopup;
using InternalAssets.Scripts.Infrastructure.UI.LoadScreen;
using InternalAssets.Scripts.Infrastructure.UI.MainMenu.CharacterLevelUp;
using InternalAssets.Scripts.Infrastructure.UI.MainMenu.CharacterSelection;
using InternalAssets.Scripts.Infrastructure.UI.MainMenu.MenuSectionsPanel;
using InternalAssets.Scripts.Infrastructure.UI.MainMenu.MapInterface;
using InternalAssets.Scripts.Infrastructure.UI.MainMenu.SettingsPopup;
using InternalAssets.Scripts.Infrastructure.UI.MainMenu.TopBar;
using InternalAssets.Scripts.Infrastructure.UI.MainMenu.WeaponUpgrade;
using InternalAssets.Scripts.Infrastructure.UI.MissionResultPopup;
using InternalAssets.Scripts.Infrastructure.UI.PausePopup;
using InternalAssets.Scripts.Infrastructure.UI.UnitsCanvas.MVC;
using Zenject;

namespace InternalAssets.Scripts.Infrastructure.Services.UISystem
{
    public class UIControllerFactory
    {
        private readonly IInstantiator _instantiator;

        public UIControllerFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        public async UniTask<IController> CreateController(UIElements uiElement)
        {
            return uiElement switch
            {
                UIElements.InfoPopup => CreateInstance<InfoPopupController>(),
                UIElements.LoadingScreen => CreateInstance<LoadingScreenController>(),
                UIElements.TopBarPanel => CreateInstance<TopBarController>(),
                UIElements.BotsMarkersPanel => CreateInstance<BotsMarkersController>(),
                UIElements.GameplayPanel => CreateInstance<GameplayPanelController>(),
                UIElements.PausePopup => CreateInstance<PausePopupController>(),
                UIElements.InfoMessagePanel => CreateInstance<InfoMessageController>(),
                UIElements.UnitsCanvas => CreateInstance<UnitsCanvasController>(),
                UIElements.CharacterLevelUpPanel => CreateInstance<CharacterLevelUpController>(),
                UIElements.CharacterSelection => CreateInstance<CharacterSelectionController>(),
                UIElements.MissionResultPopup => CreateInstance<MissionResultPopupController>(),
                UIElements.MenuSectionsPanel => CreateInstance<MenuSectionsPanelController>(),
                UIElements.MapInterface => CreateInstance<MapInterfaceController>(),
                UIElements.PlayerRewardController => CreateInstance<PlayerRewardController>(),
                UIElements.LobbySettingsPopup => CreateInstance<SettingsPopupController>(),
                UIElements.WeaponUpgradePanel => CreateInstance<WeaponUpgradeController>(),
                _ => null
            };
        }

        private IController CreateInstance<T>() where T : IController
        {
            return _instantiator.Instantiate<T>();
        }
    }
}