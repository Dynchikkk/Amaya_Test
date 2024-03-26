namespace Core.MVC
{
    public interface IUIController
    {
        public bool IsActive { get; }

        public void Init();
        public void Show();
        public void Hide();
        public void UpdateView();
    }
}
