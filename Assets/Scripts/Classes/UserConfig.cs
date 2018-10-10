using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interface
{
    [System.Serializable]
    public class UserConfig
    {
        /// <summary>
        /// CharacterConfig
        /// </summary>
        public CharacterConfig CharacterConfig { get; set; }

        /// <summary>
        /// Level Selected
        /// </summary>
        public LevelConfig LevelConfig { get; set; }
    }
}