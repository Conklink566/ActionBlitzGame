using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interface
{

    /// <summary>
    /// Character Body type for animation
    /// </summary>
    public enum CharacterBodyType
    {
        Head        = 0,
        Body        = 1,
        ArmBehind   = 2,
        Legs        = 3
    };

    /// <summary>
    /// Animation Type 
    /// </summary>
    public enum AnimationType
    {
        Idle    = 0,
        Run     = 1,
        Jump    = 2,
        Faint   = 3,
        Duck    = 4
    };

    [System.Serializable]
    public struct CharacterConfig
    {
        /// <summary>
        /// Head Type
        /// </summary>
        public int HeadType;

        /// <summary>
        /// Body Type
        /// </summary>
        public int BodyType;

        /// <summary>
        /// Legs types
        /// </summary>
        public int LegsType;

        /// <summary>
        /// ArmBehind type
        /// </summary>
        public int ArmBehindType;

        /// <summary>
        /// Character Config
        /// </summary>
        public CharacterConfig(int headType,
                               int bodyType,
                               int legsType,
                               int armBehindType)
        {
            this.HeadType = headType;
            this.BodyType = bodyType;
            this.LegsType = legsType;
            this.ArmBehindType = armBehindType;
        }

        /// <summary>
        /// Copy of an existing Character Config
        /// </summary>
        /// <returns></returns>
        public CharacterConfig Copy()
        {
            return new CharacterConfig(this.HeadType,
                                       this.BodyType,
                                       this.LegsType,
                                       this.ArmBehindType);
        }
    }
}