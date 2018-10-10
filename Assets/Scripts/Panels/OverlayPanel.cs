using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;

namespace Game.UI
{
    public class OverlayPanel : Panel
    {
        /// <summary>
        /// BAck Button
        /// </summary>
        public GameObject BackButton;


        /// <summary>
        /// Show this panel
        /// </summary>
        public override void Show()
        {
            base.Show();
        }

        /// <summary>
        /// Hide this panel
        /// </summary>
        public override void Hide()
        {
            base.Hide();
        }

        /// <summary>
        /// Check history to see if the back button needs to be displayed
        /// </summary>
        public void CheckHistory()
        {
            this.BackButton.SetActive(HUDManager.Instance.PanelDisplayHistory.Count > 1);
        }
    }
}