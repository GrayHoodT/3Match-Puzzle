using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anipang
{
    [CreateAssetMenu(fileName = "Block Configs", menuName = "Scriptable Objects/Block Configs")]
    public class BlockConfigs : ScriptableObject
    {
        [field: SerializeField]
        public List<Color> Colors { get; private set; }
    }
}
