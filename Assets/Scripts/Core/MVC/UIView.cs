using UnityEngine;

namespace Core.MVC
{
    public abstract class UIView<T> : MonoBehaviour where T : UIModel
    {
        public bool IsActive { get; private set; } = true;

        virtual public void Show()
        {
            gameObject.SetActive(true);
            IsActive = true;
        }

        virtual public void Hide()
        {
            gameObject.SetActive(false);
            IsActive = false;
        }

        virtual public void Init(T uiModel) { }

        virtual public void UpdateView(T uiModel) { }
    }
}
