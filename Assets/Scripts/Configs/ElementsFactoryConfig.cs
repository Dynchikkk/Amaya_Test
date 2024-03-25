using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ElementsFactoryConfig", menuName = "Configs/Elements Factory Config")]
    public class ElementsFactoryConfig : ScriptableObject
    {
        [field: SerializeField] public float SpawnAnimSpeed { get; private set; } = 1f;
    }
}
