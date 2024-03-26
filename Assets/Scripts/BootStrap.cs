using Core;
using Game.GamePlay;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BootStrap : MonoBehaviour
    {
        [SerializeField] private List<GameObject> Singletones = new List<GameObject>();

        [SerializeField] private ElementsFactory _elementsFactory;
        [SerializeField] private LevelController _levelController;

        private GameplayWindowController _gameplayWndowController;
        private FinishWindowController _finishWindowController;

        private void Awake()
        {
            foreach (var item in Singletones)
            {
                if (item.TryGetComponent(out ICreate creatable))
                    creatable.Create();
            }
            MainUIManager.Instance.Init();

            _finishWindowController = (FinishWindowController)MainUIManager.Instance.
                GetWindow(typeof(FinishWindowController));
            _gameplayWndowController = (GameplayWindowController)MainUIManager.Instance.
                GetWindow(typeof(GameplayWindowController));

            _levelController.Init(_elementsFactory, _gameplayWndowController);
            _levelController.OnGameEnd += _finishWindowController.Show;

            Animation();
        }

        private void Init()
        {
            _levelController.StartLevel(0, true);

            _gameplayWndowController.OnWindowFullOpen -= Init;
            _gameplayWndowController.OnWindowFullClose += Restart;
        }

        private void OnDestroy()
        {
            _levelController.OnGameEnd -= _finishWindowController.Show;
            _gameplayWndowController.OnWindowFullOpen -= Init;
            _gameplayWndowController.OnWindowFullClose -= Restart;
        }

        private void Animation()
        {
            _gameplayWndowController.OnWindowFullOpen += Init;
            _gameplayWndowController.Show();
        }

        [ContextMenu("Restart")]
        public void Restart()
        {
            Animation();
            _elementsFactory.Clear();
        }
    }
}
