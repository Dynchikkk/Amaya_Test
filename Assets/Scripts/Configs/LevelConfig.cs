using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public LevelMatrix Matrix { get; private set; } = new LevelMatrix(3, 3);
        [SerializeField] private List<ElementsPresetsConfig> _elementsPresets = new List<ElementsPresetsConfig>();

        private List<Element> _cashedElements;

        public IReadOnlyList<Element> GetAllElements()
        {
            if (_cashedElements.Count <= 0)
            {
                _cashedElements = new List<Element>();
                foreach (var presets in _elementsPresets)
                    _cashedElements.AddRange(presets.GetAllElements());
            }

            return _cashedElements;
        }

        public Element TryGetElementByName(string name)
        {
            if (_cashedElements.Count <= 0)
                GetAllElements();

            foreach (var element in _cashedElements)
            {
                if (element.Name == name)
                    return element;
            }

            return null;
        }

        /// <summary>
        /// Return list of random elements with no reapeted elements and needed element.
        /// </summary>
        public IReadOnlyList<Element> GetRandomElementNoRepeat(int count = 1)
        {
            if (_cashedElements.Count <= 0)
                GetAllElements();

            if (count <= 1)
                return new List<Element> { _cashedElements[Random.Range(0, _cashedElements.Count)] };

            if (count >= _cashedElements.Count)
                return null;

            var rnd = new System.Random();
            var elements = _cashedElements.OrderBy(x => rnd.Next()).Take(count).ToList();

            return elements;
        }

        /// <summary>
        /// Return list of random elements with no reapeted elements and needed element.
        /// If there is no needed element in dictionary return list witout needed element
        /// </summary>
        public IReadOnlyList<Element> GetRandomElementNoRepeat(string neededName, int count = 1)
        {
            var neededElement = TryGetElementByName(neededName);

            var elements = GetRandomElementNoRepeat(count);
            if (elements.Contains(neededElement))
                return elements;

            if (neededElement == null)
                return elements;

            var elementsWithNeededName = elements.ToList();
            elementsWithNeededName[0] = neededElement;
            var rnd = new System.Random();
            elementsWithNeededName = elementsWithNeededName.OrderBy(x => rnd.Next()).ToList();
            return elementsWithNeededName;
        }
    }

    [Serializable]
    public class LevelMatrix
    {
        [field: SerializeField] public int Rows { get; private set; } = 3;
        [field: SerializeField] public int Columns { get; private set; } = 3;

        public LevelMatrix(int rows, int cols)
        {
            Rows = rows;
            Columns = cols;
        }

        public int GetSize() { return Rows * Columns; }
    }
}
