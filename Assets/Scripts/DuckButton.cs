using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;
using Game.Gameplay;

namespace Game.UI
{
    public class DuckButton : EventButton
    {
        /// <summary>
        /// Button down
        /// </summary>
        private bool _ButtonDown;

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
            this._ButtonDown = true;
            Manager.Instance.Player.PlayerState = AnimationType.Duck;
        }

        /// <summary>
        /// On pointer Up
        /// </summary>
        public override void OnPointerUp()
        {
            base.OnPointerUp();
            if (!this._ButtonDown)
                return;
            this._ButtonDown = false;
            Manager.Instance.Player.PlayerState = Manager.Instance.Player.Falling ? AnimationType.Jump : AnimationType.Run;
        }

        /// <summary>
        /// On Pointer Exit
        /// </summary>
        public override void OnPointerExit()
        {
            base.OnPointerExit();
            this._ButtonDown = false;
            Manager.Instance.Player.PlayerState = Manager.Instance.Player.Falling ? AnimationType.Jump : AnimationType.Run;
        }
    }
}