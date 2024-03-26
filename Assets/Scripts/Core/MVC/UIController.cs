using UnityEngine;

namespace Core.MVC
{
    public abstract class UIController<T, T2> : MonoBehaviour, IUIController
        where T : UIView<T2>
        where T2 : UIModel, new()
    {
        public bool IsActive => _view.IsActive;

        [SerializeField] protected T _view;
        protected T2 _model = new T2();

        virtual public void Init() =>
            _view.Init(_model);

        virtual public void UpdateView() =>
            _view.UpdateView(_model);

        virtual public void Show() =>
            _view.Show();

        virtual public void Hide() =>
            _view.Hide();
    }
}
