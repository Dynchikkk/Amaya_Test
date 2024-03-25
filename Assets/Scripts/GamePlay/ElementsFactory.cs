using Configs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GamePlay
{
    public class ElementsFactory : MonoBehaviour
    {
        public event Action<string> OnElementClick = delegate { };

        [SerializeField] private ElementsFactoryConfig _config;
        [SerializeField] private ElementPlace _elementPlacePrefab;

        private readonly Dictionary<Vector2, ElementPlace> _spawned = new Dictionary<Vector2, ElementPlace>();

        public void SpawnElements(LevelConfig levelConfig, bool smooth = false, string neededName = "")
        {
            var elSizeX = _elementPlacePrefab.GetSize().x;
            var elSizeY = _elementPlacePrefab.GetSize().y;

            var matrix = levelConfig.Matrix;
            var elements = levelConfig.GetRandomElementNoRepeat(neededName, matrix.GetSize());

            if (_spawned.Count % 2 != matrix.GetSize() % 2)
                ClearSpawned();

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
                        element.OnElementClick += RequestForCheckÑorrectness;
                    }

                    tempOffset.x += elSizeX;
                    indexOfElement++;
                }
                tempOffset.x = constOffset.x;
                tempOffset.y -= elSizeY;
            }
        }

        private void OnDestroy()
        {
            foreach (var element in _spawned.Values)
                element.OnElementClick -= RequestForCheckÑorrectness;
        }

        private void RequestForCheckÑorrectness(string name) =>
            OnElementClick?.Invoke(name);

        private void ClearSpawned()
        {
            foreach (var item in _spawned)
                Destroy(item.Value.gameObject);

            _spawned.Clear();
        }
    }
}
