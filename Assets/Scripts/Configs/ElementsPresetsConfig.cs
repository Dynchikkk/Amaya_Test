using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ElementsPresetConfig", menuName = "Configs/Elements Preset Config")]
    public class ElementsPresetsConfig : ScriptableObject
    {
        [SerializeField] private Element[] _elements = new Element[1];

        public IReadOnlyList<Element> GetAllElements() { return _elements.ToList(); }

        public Element TryGetElementByName(string name)
        {
            foreach (var element in _elements)
            {
                if (element.Name == name)
                    return element;
            }

            return null;
        }
    }

    [Serializable]
    public class Element
    {
        [field: SerializeField] public string Name { get; private set; } = "default";
        [field: SerializeField] public Sprite Icon { get; private set; } = null;
        [field: SerializeField] public float RotationClockwise { get; private set; } = .0f;

        public Element(string name, Sprite icon, float rotationClockwise)
        {
            Name = name;
            Icon = icon;
            RotationClockwise = rotationClockwise;
        }
    }
}
