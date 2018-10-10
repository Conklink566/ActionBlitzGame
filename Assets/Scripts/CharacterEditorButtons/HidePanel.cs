using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class HidePanel : MonoBehaviour
    {

        /// <summary>
        /// Check to see if pointer is down
        /// </summary>
        private bool _PointerDown = false;

        /// <summary>
        /// Selected Panel that will be hidden
        /// </summary>
        public GameObject SelectedPanel;

        /// <summary>
        /// On Pointer Exit
        /// </summary>
        public void OnPointerExit()
        {
            this._PointerDown = false;
        }

        /// <summary>
        /// On Pointer Down
        /// </summary>
        public void OnPointerDown()
        {
            this._PointerDown = true;
        }

        /// <summary>
        /// On Pointer Up
        /// </summary>
        public void OnPointerUp()
        {
            if (!this._PointerDown)
                return;
            this._PointerDown = false;
            this.SelectedPanel.SetActive(false);
        }
    }
}