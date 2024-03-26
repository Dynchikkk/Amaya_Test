using Core.MVC;
using System;

namespace Game.UI
{
    public class GameplayWindowController : UIController<GameplayWindowView, GameplayWindowModel>
    {
        public event Action OnWindowFullOpen = delegate { };
        public event Action OnWindowFullClose = delegate { };

        public override void Init()
        {
            base.Init();
            _view.OnWindowFullOpen += RequestForOpen;
            _view.OnWindowFullClose += RequestForClose;
        }

        private void OnDestroy()
        {
            _view.OnWindowFullOpen -= RequestForOpen;
            _view.OnWindowFullClose -= RequestForClose;
        }

        public void UpdateFindedElement(string name)
        {
            _model = new GameplayWindowModel() { ElementName = name};
            UpdateView();
        }

        public void HideWithAnimation() => _view.HideWithAnimation();

        private void RequestForOpen() => OnWindowFullOpen?.Invoke();
        private void RequestForClose() => OnWindowFullClose?.Invoke();
    }
}
