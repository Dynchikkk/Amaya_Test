using UnityEngine;

namespace Core.MVC
{
    public abstract class UIView<T> : MonoBehaviour where T : UIModel
    {
        public bool IsActive { get; private set; } = true;

        virtual public void Show()
        {
            for (var i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);

            IsActive = true;
        }

        virtual public void Hide()
        {
            for (var i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);

            IsActive = false;
        }

        virtual public void Init(T uiModel) { }

        virtual public void UpdateView(T uiModel) { }
    }
}
