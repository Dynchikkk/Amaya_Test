using Core.MVC;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class GameplayWindowView : UIView<GameplayWindowModel>
    {
        public event Action OnWindowFullOpen = delegate { };
        public event Action OnWindowFullClose = delegate { };

        [SerializeField] private RectTransform _transitionImage;
        [SerializeField] private TMP_Text _findElementText;

        [Header("Animation")]
        [SerializeField] private float _animationSpeed = 1;

        public override void Show()
        {
            ResetText();
            base.Show();
            StartCoroutine(OpenAnimation());
        }

        public override void Hide()
        {
            base.Hide();
            OnWindowFullClose?.Invoke();
        }

        public void HideWithAnimation() =>
            StartCoroutine(CloseAnimation());

        public override void UpdateView(GameplayWindowModel uiModel)
        {
            _findElementText.gameObject.SetActive(true);
            _findElementText.DOFade(1, _animationSpeed);
            _findElementText.text = "Find " + uiModel.ElementName;
        }

        private IEnumerator OpenAnimation()
        {
            _transitionImage.DOScale(Vector2.one * 20, 0);

            _transitionImage.DOScale(Vector2.zero, _animationSpeed);
            yield return new WaitForSeconds(_animationSpeed / 2);
            OnWindowFullOpen?.Invoke();
        }

        private IEnumerator CloseAnimation()
        {
            _transitionImage.DOScale(Vector2.one * 20, _animationSpeed);
            yield return new WaitForSeconds(_animationSpeed);
            //base.Hide();
            OnWindowFullClose?.Invoke();
        }

        private void ResetText()
        {
            _findElementText.text = "";
            _findElementText.DOFade(0, 0);
            _findElementText.gameObject.SetActive(false);
        }
    }

    public class GameplayWindowModel : UIModel
    {
        public string ElementName { get; set; }
    }
}
