using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;

namespace Game.UI
{
    public class DisplayAnimationButton : EventButton
    {
        /// <summary>
        /// Animation Type
        /// </summary>
        public AnimationType AnimationType;

        /// <summary>
        /// Character Display
        /// </summary>
        public CharacterDisplay CharacterDisplay;

        /// <summary>
        /// Enabled Color
        /// </summary>
        public Color EnabledColor;

        /// <summary>
        /// Disabled Color
        /// </summary>
        public Color DisabledColor;

        /// <summary>
        /// Frame that changes colors based off toggle
        /// </summary>
        public Image ButtonFrame;

        /// <summary>
        /// Selected Button
        /// </summary>
        public bool SelectedButton = false;

        /// <summary>
        /// Button Down
        /// </summary>
        public bool ButtonDown { get; set; }

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
            this.ButtonDown = true;
        }

        /// <summary>
        /// On Pointer Exit
        /// </summary>
        public override void OnPointerExit()
        {
            base.OnPointerExit();
            if (!this.ButtonInteraction)
                return;
            this.ButtonDown = false;
        }

        /// <summary>
        /// On Pointer Up
        /// </summary>
        public override void OnPointerUp()
        {
            base.OnPointerUp();
            if (!this.ButtonInteraction || 
                !this.ButtonDown)
                return;
            this.ButtonDown = false;
            this.CharacterDisplay.CharacterAnimationSelection(this.SelectedButton ? AnimationType.Idle : this.AnimationType);
            this.SelectedButton = !this.SelectedButton;
        }

        /// <summary>
        /// Toggle this button to be enabled or disabled
        /// </summary>
        public void ButtonToggle(bool toggle)
        {
            this.ButtonFrame.color = toggle ? this.EnabledColor : this.DisabledColor;
        }
    }
}