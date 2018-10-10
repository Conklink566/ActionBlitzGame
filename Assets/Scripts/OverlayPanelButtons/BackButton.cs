using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game.UI
{
    public class BackButton : EventButton
    {
        /// <summary>
        /// ?Button Down
        /// </summary>
        private bool _ButtonDown = false;

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
            //Go Back to previous Panel
            if (HUDManager.Instance.CurrentlyDisplayPanel as CharacterEditPanel)
            {
                CharacterEditPanel panel = HUDManager.Instance.CurrentlyDisplayPanel as CharacterEditPanel;
                if (panel.ChangesMade)
                {
                    panel.SaveWarningPanel.SetActive(true);
                    return;
                }
            }
            if(HUDManager.Instance.CurrentlyDisplayPanel as LevelSelectionPanel)
            {
                FileConfigHandler.Instance.UserConfig.LevelConfig = HUDManager.Instance.SelectedLevelConfig;
                FileConfigHandler.Save();
            }

            HUDManager.Instance.RemovePanelToList(true);
        }

        /// <summary>
        /// On Pointer Exit
        /// </summary>
        public override void OnPointerExit()
        {
            base.OnPointerExit();
            this._ButtonDown = false;
        }
    }
}