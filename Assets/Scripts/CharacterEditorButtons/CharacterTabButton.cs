using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CharacterTabButton : EventButton
    {
        /// <summary>
        /// The image frame that will change colors to show if the button is active or not
        /// </summary>
        public Image ButtonFrame;

        /// <summary>
        /// Enabled Button Color for when it is interactable
        /// </summary>
        public Color EnabledColor;

        /// <summary>
        /// Disabled button color for when it is interactable
        /// </summary>
        public Color DisabledColor;

        /// <summary>
        /// Character Edit Panel
        /// </summary>
        public CharacterEditPanel CharacterEditPanel;

        /// <summary>
        /// Selected Editor type from the editor
        /// </summary>
        public EditorTypes CurrentEditorType;

        /// <summary>
        /// Checked to see if the button Down
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
        /// OnPointerUp
        /// </summary>
        public override void OnPointerUp()
        {
            base.OnPointerUp();
            if(!this.ButtonInteraction ||
               !this._ButtonDown)
            {
                return;
            }
            this.CharacterEditPanel.SelectEditorType(this.CurrentEditorType);
        }

        /// <summary>
        /// Toggle this button to be enabled or disabled
        /// </summary>
        public void ButtonToggle(bool toggle)
        {
            this.ButtonFrame.color = toggle ? this.EnabledColor : this.DisabledColor;
            this.ButtonInteraction = toggle;
        }
    }
}