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


        public IReadOnlyList<Element> GetAllElements()
        {
            List<Element> elements = new List<Element>();
            foreach (var presets in _elementsPresets)
                elements.AddRange(presets.GetAllElements());
 
            return elements;
        }

        public Element TryGetElementByName(string name)
        {
            var elements = GetAllElements();

            foreach (var element in elements)
            {
                if (element.Name == name)
                    return element;
            }

            return null;
        }

        /// <summary>
        /// Return list of random elements with no reapeted elements and needed element.
        /// </summary>
        public IReadOnlyList<Element> GetRandomElementsNoRepeat(int count = 1)
        {
            var allElements = GetAllElements();

            if (count <= 1)
                return new List<Element> { allElements[Random.Range(0, allElements.Count)] };

            if (count >= allElements.Count)
                return null;

            var rnd = new System.Random();
            var elements = allElements.OrderBy(x => rnd.Next()).Take(count).ToList();

            return elements;
        }

        /// <summary>
        /// Return list of random elements with no reapeted elements and needed element.
        /// If there is no needed element in dictionary return list witout needed element
        /// </summary>
        public IReadOnlyList<Element> GetRandomElementNoRepeat(string neededName, int count = 1)
        {
            var neededElement = TryGetElementByName(neededName);

            var elements = GetRandomElementsNoRepeat(count);
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

        public Element GetRandomElementNoRepeat(IReadOnlyList<string> bannedNames)
        {
            var elementsWithNoRepaet = new List<Element>();
            var allElements = GetAllElements();
            foreach (var element in allElements)
            {
                if (bannedNames.Contains(element.Name))
                    continue;
                elementsWithNoRepaet.Add(element);
            }

            return elementsWithNoRepaet[Random.Range(0, elementsWithNoRepaet.Count)];
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
