using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;

namespace Game.UI
{
    public class CharacterOutfitButton : MonoBehaviour
    {
        /// <summary>
        /// Button Down
        /// </summary>
        private bool _ButtonDown = false;

        /// <summary>
        /// when this button is not selected and can be interacted
        /// </summary>
        public Color EnabledColor;

        /// <summary>
        /// When this button is selected and can't be interacted
        /// </summary>
        public Color DisabledColor;

        /// <summary>
        /// Button Icon
        /// </summary>
        public Image ButtonIcon;

        /// <summary>
        /// Button Frame
        /// </summary>
        public Image ButtonFrame;

        /// <summary>
        /// Button Interaction
        /// </summary>
        public bool ButtonInteraction = true;

        /// <summary>
        /// Character Display
        /// </summary>
        private CharacterDisplay _CharacterDisplay;

        /// <summary>
        /// Character Body Type
        /// </summary>
        public CharacterBodyType CharacterBodyType { get; set; }

        /// <summary>
        /// Body Type
        /// </summary>
        public int BodyType { get; set; }

        /// <summary>
        /// OnPointerDown
        /// </summary>
        public void OnPointerDown()
        {
            this._ButtonDown = true;
        }

        /// <summary>
        /// OnPointerExit
        /// </summary>
        public void OnPointerExit()
        {
            this._ButtonDown = false;
        }

        /// <summary>
        /// OnPointerUp
        /// </summary>
        public void OnPointerUp()
        {
            if(!this._ButtonDown ||
               !this.ButtonInteraction)
            {
                return;
            }
            this.ButtonToggle(false);
            this._CharacterDisplay.CharacterEditPanel.ChangesMade = true;
            this._CharacterDisplay.ChangeAnimatorLayerSet(this.CharacterBodyType, this.BodyType, true, this.ButtonIcon.sprite);
        }

        /// <summary>
        /// Button Toggle
        /// </summary>
        public void ButtonToggle(bool toggle)
        {
            this.ButtonInteraction = toggle;
            this.ButtonFrame.color = toggle ? this.EnabledColor : this.DisabledColor;
        }

        /// <summary>
        /// Data Bind
        /// </summary>
        public void DataBind(Sprite icon, CharacterDisplay characterDisplay, CharacterBodyType bodyType, int type)
        {
            this._CharacterDisplay = characterDisplay;
            this.ButtonIcon.sprite = icon;
            this.BodyType = type;
            this.CharacterBodyType = bodyType;
            switch(bodyType)
            {
                case CharacterBodyType.Head:
                    if(this._CharacterDisplay.CharacterEditPanel.CharacterConfig.HeadType == type)
                    {
                        this.ButtonToggle(false);
                    }
                    break;
                case CharacterBodyType.Body:
                    if (this._CharacterDisplay.CharacterEditPanel.CharacterConfig.BodyType == type)
                    {
                        this.ButtonToggle(false);
                    }
                    break;
                case CharacterBodyType.Legs:
                    if (this._CharacterDisplay.CharacterEditPanel.CharacterConfig.LegsType == type)
                    {
                        this.ButtonToggle(false);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}