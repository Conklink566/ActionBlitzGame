using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class CharacterCancelButton : EventButton
    {

        /// <summary>
        /// Check to see if button is down
        /// </summary>
        private bool _ButtonDown;

        /// <summary>
        /// Selected Panel that will be hidden
        /// </summary>
        public GameObject SelectedPanel;

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
            if (!this._ButtonInteraction)
                return;
            this._ButtonDown = true;
        }

        /// <summary>
        /// On Pointer Exit
        /// </summary>
        public override void OnPointerExit()
        {
            base.OnPointerExit();
            this._ButtonDown = false;
        }

        /// <summary>
        /// On Pointer UP
        /// </summary>
        public override void OnPointerUp()
        {
            base.OnPointerUp();
            if (!this._ButtonInteraction ||
               !this._ButtonDown)
                return;
            this.SelectedPanel.SetActive(false);
        }
    }
}