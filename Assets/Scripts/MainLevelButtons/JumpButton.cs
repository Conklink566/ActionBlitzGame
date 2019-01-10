using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Gameplay;
using Game.Interface;

namespace Game.UI
{
    public class JumpButton : EventButton
    { 
        /// <summary>
        /// Awake this instance
        /// </summary>
        public override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// On Pointer Down
        /// </summary>
        public override void OnPointerDown()
        {
            base.OnPointerDown();
            Manager.Instance.Player.Jump();
            AudioManager.Instance.CreateSoundEffect(SoundEffectType.Jump, Manager.Instance.PlayerFollow.transform.position);
        }

        /// <summary>
        /// On pointer Up
        /// </summary>
        public override void OnPointerUp()
        {
            base.OnPointerUp();
        }

        /// <summary>
        /// On Pointer Exit
        /// </summary>
        public override void OnPointerExit()
        {
            base.OnPointerExit();
        }
    }
}