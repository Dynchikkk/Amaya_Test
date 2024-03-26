using Configs;
using Game.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private List<LevelConfig> _levels;
        private ElementsFactory _elementsFactory;


    }
}
