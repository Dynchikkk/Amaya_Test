using Configs;
using Game.GamePlay;
using UnityEngine;

namespace Game
{
    public class BootStrap : MonoBehaviour
    {
        [SerializeField] private ElementsFactory _elementsFactory;

        [SerializeField] private LevelConfig _testLevel;
        [SerializeField] private LevelConfig _testLevel2;

        private void Start()
        {
            _elementsFactory.SpawnElements(_testLevel, true);
            _elementsFactory.OnElementClick += Test;
        }

        [ContextMenu("Test")]
        private void Test1()
        {
            _elementsFactory.SpawnElements(_testLevel2, neededName: "7");
        }

        private void Test(string name)
        {
            Debug.Log(name);
        }
    }
}
