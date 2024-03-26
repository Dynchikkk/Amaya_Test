using System;
using System.Collections.Generic;

namespace Core.MVC
{
    public abstract class UIManager<TM> : Singleton<TM>
        where TM : UIManager<TM>
    {
        protected Dictionary<Type, IUIController> _controllers = new Dictionary<Type, IUIController>();

        protected abstract void AddControllers();

        public virtual void Init()
        {
            AddControllers();
            InitAll();
            HideAll();
        }

        protected virtual void InitAll()
        {
            foreach (var controller in _controllers.Values)
                controller.Init();
        }

        protected virtual void HideAll()
        {
            foreach (var controllerPair in _controllers)
            {
                if (controllerPair.Value.IsActive)
                    controllerPair.Value.Hide();
            }
        }

        public IUIController ShowWindow(Type T, bool hideOtherWindows = true)
        {
            if (hideOtherWindows)
                HideAll();

            var controller = _controllers[T];
            controller.Show();
            return controller;
        }

        public IUIController GetWindow(Type T)
        {
            var controller = _controllers[T];
            return controller;
        }
    }
}
