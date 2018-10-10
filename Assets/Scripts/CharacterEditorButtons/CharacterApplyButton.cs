using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game.UI
{
    public class CharacterApplyButton : EventButton
    {
        /// <summary>
        /// Character Edit Panel
        /// </summary>
        public CharacterEditPanel CharacterEditPanel;

        /// <summary>
        /// Check to see if the button is pressed down
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
        /// OnPointerDown
        /// </summary>
        public override void OnPointerDown()
        {
            base.OnPointerDown();
            if (!this.ButtonInteraction)
                return;
            this._ButtonDown = true;
        }

        /// <summary>
        /// OnPointerExit
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
            this._ButtonDown = false;
            this.CharacterEditPanel.CharacterDisplay.ClearButtons();
            HUDManager.Instance.SelectedCharacterConfig = this.CharacterEditPanel.CharacterConfig.Copy();
            FileConfigHandler.Instance.UserConfig.CharacterConfig = this.CharacterEditPanel.CharacterConfig.Copy();
            FileConfigHandler.Save();
            //Go Back to previous Panel
            HUDManager.Instance.RemovePanelToList(true);
        }
    }
}