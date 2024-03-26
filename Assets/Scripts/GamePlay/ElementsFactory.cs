using Configs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GamePlay
{
    public class ElementsFactory : MonoBehaviour
    {
        public event Action<string> OnElementClick = delegate { };
        public event Action OnElementsShow = delegate { };

        [SerializeField] private ElementsFactoryConfig _config;
        [SerializeField] private ElementPlace _elementPlacePrefab;

        private readonly Dictionary<Vector2, ElementPlace> _spawned = new Dictionary<Vector2, ElementPlace>();
        private int _lastRows = 0, _lastColumns = 0;

        private void OnDestroy()
        {
            foreach (var element in _spawned.Values)
                element.OnElementClick -= RequestForCheck—orrectness;
        }

        public void SpawnElements(LevelConfig levelConfig, bool smooth = false, string neededName = "")
        {
            transform.localScale = Vector3.one;

            var matrix = levelConfig.Matrix;
            var elements = levelConfig.GetRandomElementsNoRepeat(neededName, matrix.GetSize());

            var elSizeX = _elementPlacePrefab.GetSize().x;
            var elSizeY = _elementPlacePrefab.GetSize().y;

            if ((_lastRows % 2 != matrix.Rows % 2) || (_lastColumns % 2 != matrix.Columns % 2))
                Clear();

            _lastRows = matrix.Rows;
            _lastColumns = matrix.Columns;

            // Calculate offset
            var constOffset = Vector2.zero;
            constOffset.x -= (matrix.Columns / 2) * elSizeX;
            constOffset.y += (matrix.Rows / 2) * elSizeY;
            if (matrix.Columns % 2 == 0) constOffset.x += elSizeX / 2;
            if (matrix.Rows % 2 == 0) constOffset.y -= elSizeY / 2;

            // Spawn
            int indexOfElement = 0;
            var tempOffset = constOffset;
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int y = 0; y < matrix.Columns; y++)
                {
                    var elConfig = elements[indexOfElement];

                    // Check if element has been already spawned
                    // if yes -> change params
                    // if no -> spawn and change params
                    if (_spawned.TryGetValue(tempOffset, out ElementPlace element))
                        element.ChangeElement(elConfig);
                    else
                    {
                        element = Instantiate(_elementPlacePrefab, tempOffset, Quaternion.identity, transform);
                        if (smooth)
                            element.Spawn(elConfig, _config.SpawnAnimSpeed);
                        else
                            element.Spawn(elConfig);
                        _spawned.Add(tempOffset, element);
                        element.OnElementClick += RequestForCheck—orrectness;
                    }

                    tempOffset.x += elSizeX;
                    indexOfElement++;
                }
                tempOffset.x = constOffset.x;
                tempOffset.y -= elSizeY;
            }

            ScaleField(matrix);
            ChangeElementsCondition(true);

            OnElementsShow?.Invoke();
        }

        public void ChooseElement(string name, bool isRight)
        {
            foreach (var element in _spawned.Values)
            {
                if (element.ElementName == name)
                {
                    if (isRight)
                        element.DoOnRightChoose();
                    else
                        element.DoOnWrongChoose();
                    return;
                }
            }
        }

        private void ScaleField(LevelMatrix levelMatrix)
        {
            var scaleCoef = levelMatrix.Rows - 3;
            scaleCoef = scaleCoef > levelMatrix.Columns ? scaleCoef : levelMatrix.Columns - 3;
            scaleCoef = Mathf.Clamp(scaleCoef, 0, levelMatrix.GetSize());
            transform.localScale = (float)Math.Pow(_config.ScaleStrength, scaleCoef) * Vector3.one;
        }

        public void ChangeElementsCondition(bool condition)
        {
            foreach (var element in _spawned.Values)
                element.IsActive = condition;
        }

        public void Clear()
        {
            foreach (var item in _spawned)
                Destroy(item.Value.gameObject);

            _spawned.Clear();
        }

        private void RequestForCheck—orrectness(string name) =>
            OnElementClick?.Invoke(name);
    }
}
