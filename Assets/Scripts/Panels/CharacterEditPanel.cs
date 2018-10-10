using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game.UI
{
    /// <summary>
    /// Editor Types
    /// </summary>
    public enum EditorTypes
    {
        CharacterEditor = 0,
        AccSkillEditor  = 1
    };

    public class CharacterEditPanel : Panel
    {
        /// <summary>
        /// Accessory Display Holder
        /// </summary>
        public GameObject AccessoryDisplayHolder;

        /// <summary>
        /// Character Accessory Holder
        /// </summary>
        public GameObject CharacterAccessoryHolder;

        /// <summary>
        /// Save Warning Panel
        /// </summary>
        public GameObject SaveWarningPanel;

        /// <summary>
        /// Character Tab Button
        /// </summary>
        public CharacterTabButton[] CharacterTabButton;

        /// <summary>
        /// Character Config
        /// </summary>
        public CharacterConfig CharacterConfig;

        /// <summary>
        /// Character Display
        /// </summary>
        public CharacterDisplay CharacterDisplay;

        /// <summary>
        /// Current Editor Type
        /// </summary>
        public EditorTypes CurrentEditorType;

        /// <summary>
        /// Check to see if there has been an edit to the character
        /// </summary>
        public bool ChangesMade;

        /// <summary>
        /// Show Panel
        /// </summary>
        public override void Show()
        {
            base.Show();
            this.CharacterConfig = HUDManager.Instance.SelectedCharacterConfig.Copy();
            this.SelectEditorType(EditorTypes.CharacterEditor);
            this.SaveWarningPanel.SetActive(false);
            this.ChangesMade = false;
        }

        /// <summary>
        /// Hide Panel
        /// </summary>
        public override void Hide()
        {
            base.Hide();
        }

        /// <summary>
        /// Select the type of editor
        /// </summary>
        public void SelectEditorType(EditorTypes type)
        {
            this.CurrentEditorType = type;
            for(int i = 0; i < this.CharacterTabButton.Length; i++)
            {
                if (this.CharacterTabButton[i].CurrentEditorType != type)
                    this.CharacterTabButton[i].ButtonToggle(true);
                else
                    this.CharacterTabButton[i].ButtonToggle(false);
            }

            this.CharacterDisplay.ShowPanel(EditorTypes.CharacterEditor == type);
            this.AccessoryDisplayHolder.SetActive(EditorTypes.AccSkillEditor == type);
            this.CharacterAccessoryHolder.SetActive(EditorTypes.AccSkillEditor == type);
        }
    }
}