using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game.UI
{
    public class Panel : MonoBehaviour
    {
        /// <summary>
        /// Start this instance
        /// </summary>
        public virtual void Show()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}