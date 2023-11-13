using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using InternalAssets.Scripts.Infrastructure.Services;
using InternalAssets.Scripts.Infrastructure.Services.UISystem;

namespace ECGaming.UISystem.Interfaces
{
    public interface IUIService : IService
    {
        bool IsReady { get; }

        bool StateIsLoaded(UIStateConfigType uiStateConfig);

        /// <summary>
        /// Создаёт экземпляр на сцене UI Root
        /// </summary>
        /// <returns></returns>
        UniTask CreateUIRoot();
        
        /// <summary>
        /// Загружает все View нашего стейта и создаём контроллеры, которые потом необходимо проинциализировать
        /// </summary>
        /// <param name="uiStateConfig"></param>
        /// <returns></returns>
        UniTask Preload(UIStateConfigType uiState);
        
        /// <summary>
        /// Очищает все контроллеры и удаляем View
        /// </summary>
        /// <param name="uiState"></param>
        /// <returns></returns>
        UniTask ReleaseState(UIStateConfigType uiState);
        
        /// <summary>
        /// Вытаскивает необходимый контроллер по типу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetController<T>() where T : IController;

        /// <summary>
        /// Отдает все контроллеры
        /// </summary>
        List<IController> GetAllControllers();

        /// <summary>
        /// Активирует View по типу контроллера
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ShowUIElement<T>() where T : IController;

        /// <summary>
        /// Активирует все View указанного UIStateConfig
        /// </summary>
        /// <param name="uiStateConfig"></param>
        public void ShowState(UIStateConfigType uiStateConfig);

        /// <summary>
        /// Показывает элементы нового UIStateConfig, если они не показаны
        /// </summary>
        /// <param name="uiStateConfig"></param>
        public void ShowStateWithMerge(UIStateConfigType uiStateConfig);

        /// <summary>
        /// Деактивирует все View указанного UIStateConfig 
        /// </summary>
        /// <param name="uiStateConfig"></param>
        public void HideState(UIStateConfigType uiStateConfig);

        /// <summary>
        /// Скрывает элементы, которых нет в новом uiStateConfig
        /// </summary>
        /// <param name="uiStateConfig"></param>
        void HideStateWithMerge(UIStateConfigType uiState);

        /// <summary>
        /// Деактивирует конкретный View по типу контроллера 
        /// </summary>
        /// <param name="uiStateConfig"></param>
        public void HideUIElement<T>() where T : IController;

        /// <summary>
        /// Обновляет данные в Model контроллера по типу.
        /// Если useSilentUpdate = true, то обновление произойдёт без уведомления View 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="useSilentUpdate"></param>
        /// <typeparam name="T"></typeparam>
        public void UpdateUIElementModel<T>(string propertyName, object value, bool useSilentUpdate = false) where T : IController;

        /// <summary>
        /// Обновляет данные во View по типу контроллера без изменения данных Model
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public void UpdateUIElementView<T>(string propertyName, object value) where T : IController;
    }
}