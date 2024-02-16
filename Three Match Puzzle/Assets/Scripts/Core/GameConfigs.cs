using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anipang
{
    [CreateAssetMenu(fileName = "Game Configs", menuName = "Scriptable Objects/Game Configs")]
    public class GameConfigs : ScriptableObject
    {
        [field: SerializeField]
        public int Row { get; private set; }
        [field: SerializeField] 
        public int Column { get; private set; }
        [field: SerializeField]
        public float limitedTime { get; private set; }
    }
}

