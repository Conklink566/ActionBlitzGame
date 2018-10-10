using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game.UI
{
    public class CharacterDiscardButton : EventButton
    {

        /// <summary>
        /// Check to see if the button is down
        /// </summary>
        private bool _ButtonDown = false;

        /// <summary>
        /// Character Edit Panel
        /// </summary>
        public CharacterEditPanel CharacterEditPanel;

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
            if (!this.ButtonInteraction)
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
        /// On Pointer Up
        /// </summary>
        public override void OnPointerUp()
        {
            base.OnPointerUp();
            if (!this.ButtonInteraction ||
                !this._ButtonDown)
                return;
            this.CharacterEditPanel.CharacterDisplay.ClearButtons();
            HUDManager.Instance.RemovePanelToList(true);
        }
    }
}