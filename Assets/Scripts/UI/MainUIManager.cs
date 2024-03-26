using Core.MVC;
using UnityEngine;

namespace Game.UI
{
    public class MainUIManager : UIManager<MainUIManager>
    {
        [Header("Controllers")]
        [SerializeField] private GameplayWindowController _gameplayWindowController;
        [SerializeField] private FinishWindowController _finishWindowController;

        public override void Init()
        {
            base.Init();
            _finishWindowController.OnRestart += _gameplayWindowController.HideWithAnimation;
        }

        private void OnDestroy() =>
            _finishWindowController.OnRestart -= _gameplayWindowController.HideWithAnimation;

        protected override void AddControllers()
        {
            _controllers.Add(_gameplayWindowController.GetType(), _gameplayWindowController);
            _controllers.Add(_finishWindowController.GetType(), _finishWindowController);
        }
    }
}
