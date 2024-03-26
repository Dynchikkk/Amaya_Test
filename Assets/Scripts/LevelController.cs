using Configs;
using Game.GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelController : MonoBehaviour
    {
        public event Action OnGameEnd = delegate { };

        public int CurrentLevel => _currentLevel;
        public string CurrentNameToFind => _namesToFind[_currentLevel];

        [SerializeField] private List<LevelConfig> _levels;
        [SerializeField] private float _defaultWaitTimeBetweenLevels = 1f;

        private ElementsFactory _elementsFactory;
        private int _currentLevel = 0;

        private List<string> _namesToFind;

        public void Init(ElementsFactory factory)
        {
            _namesToFind = new List<string>(_levels.Count);
            FillNonRepeatNamesToFind();
            _currentLevel = 0;

            _elementsFactory = factory;
            _elementsFactory.OnElementClick += CheckAnswer;
        }

        private void OnDestroy()
        {
            _elementsFactory.OnElementClick -= CheckAnswer;
            StopAllCoroutines();
        }

        public void StartLevel(int num = 0, bool smooth = false, float waitTimeBetweenLevels = -1)
        {
            var time = waitTimeBetweenLevels == -1 ? _defaultWaitTimeBetweenLevels : waitTimeBetweenLevels;
            StartCoroutine(StartLevelCuro(num, smooth, time));
        }

        private IEnumerator StartLevelCuro(int num, bool smooth, float waitTimeBetweenLevels)
        {
            yield return new WaitForSeconds(waitTimeBetweenLevels);
            _currentLevel = num;
            _elementsFactory.SpawnElements(_levels[num], smooth, CurrentNameToFind);
            Debug.Log($"Find {CurrentNameToFind}");
        }

        private void CheckAnswer(string name)
        {
            var result = name == CurrentNameToFind;

            _elementsFactory.ChooseElement(name, result);
            if (!result)
                return;

            _currentLevel++;
            _elementsFactory.ChangeElemetsCondition(false);

            if (_currentLevel >= _levels.Count)
            {
                OnGameEnd?.Invoke();
                _currentLevel = 0;
            }
            else
                StartLevel(_currentLevel);
        }

        private void FillNonRepeatNamesToFind()
        {
            foreach (var level in _levels) 
                _namesToFind.Add(level.GetRandomElementNoRepeat(_namesToFind).Name);
        }
    }
}
