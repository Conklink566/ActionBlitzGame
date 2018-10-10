using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game.UI
{
    public class SettingsButton : EventButton
    {
        /// <summary>
        /// HUDManager instance
        /// </summary>
        private HUDManager _HUDManager;

        /// <summary>
        /// HUDManager Property
        /// </summary>
        public HUDManager HUDManager
        {
            get
            {
                if (this._HUDManager == null)
                    this._HUDManager = HUDManager.Instance;
                return this._HUDManager;
            }
        }

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
        }

        /// <summary>
        /// On pointer Up
        /// </summary>
        public override void OnPointerUp()
        {
            base.OnPointerUp();
            if (!this.EventDown)
                return;
            this.EventDown = false;
            //Go to Settings Panel
            this.HUDManager.AddPanelToList(this.HUDManager.SettingsPanel, true);
        }

        /// <summary>
        /// On Pointer Exit
        /// </summary>
        public override void OnPointerExit()
        {
            base.OnPointerExit();
        }
    }
}
