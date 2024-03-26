using Configs;
using Game.GamePlay;
using UnityEngine;

namespace Game
{
    public class BootStrap : MonoBehaviour
    {
        [SerializeField] private ElementsFactory _elementsFactory;
        [SerializeField] private LevelController _levelController;

        private void Awake() => Init();

        private void Init()
        {
            _levelController.Init(_elementsFactory);
            _levelController.StartLevel(0, true);
        }

        [ContextMenu("Restart")]
        public void Restart()
        {
            _elementsFactory.Clear();
            Init();
        }
    }
}
