using Core.MVC;
using System;

namespace Game.UI
{
    public class FinishWindowController : UIController<FinishWindowView, FinishWindowModel>
    {
        public event Action OnRestart;

        public override void Init()
        {
            base.Init();
            _view.OnRestart += RequestForRestart;
        }

        private void OnDestroy() =>
            _view.OnRestart -= RequestForRestart;

        private void RequestForRestart() => OnRestart?.Invoke();
    }
}
