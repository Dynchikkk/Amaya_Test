using Core.MVC;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FinishWindowView : UIView<FinishWindowModel>
    {
        public event Action OnRestart = delegate { };

        [SerializeField] private Image _background;
        [SerializeField] private Button _restartButton;

        [Header("Animation")]
        [SerializeField] private float _animationSpeed = 1;

        public override void Init(FinishWindowModel uiModel) =>
            _restartButton.onClick.AddListener(RequestForRestart);

        private void OnDestroy() =>
            _restartButton.onClick.RemoveListener(RequestForRestart);

        public override void Show()
        {
            _background.DOFade(0, 0);
            base.Show();
            _background.DOFade(0.5f, _animationSpeed);
        }

        private void RequestForRestart()
        {
            OnRestart?.Invoke();
            Hide();
        }
    }

    public class FinishWindowModel : UIModel { }
}
