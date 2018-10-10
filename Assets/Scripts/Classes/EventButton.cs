using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class EventButton : MonoBehaviour
    {
        /// <summary>
        /// Original Size
        /// </summary>
        private Vector2 _OriginalSize;

        /// <summary>
        /// Rect Transform
        /// </summary>
        public RectTransform AssemblyTransform;

        /// <summary>
        /// Scale Adjustment to the Button
        /// </summary>
        public float AdjustmentScale;
        
        /// <summary>
        /// Enable/Disable button interactions
        /// </summary>
        public bool _ButtonInteraction = true;

        /// <summary>
        /// Button interaction Property
        /// </summary>
        public bool ButtonInteraction
        {
            get
            {
                return this._ButtonInteraction;
            }
            set
            {
                this._ButtonInteraction = value;
                if(this._OriginalSize == Vector2.zero)
                    this._OriginalSize = this.AssemblyTransform.sizeDelta;
                this.AssemblyTransform.sizeDelta = this._OriginalSize;
                this.EventDown = false;
            }
        }


        /// <summary>
        /// Button down
        /// </summary>
        public bool EventDown { get; set; }

        /// <summary>
        /// Awake this instance
        /// </summary>
        public virtual void Awake()
        {
            this.EventDown = false;
            this._OriginalSize = this.AssemblyTransform.sizeDelta;
        }

        /// <summary>
        /// On Pointer Down
        /// </summary>
        public virtual void OnPointerDown()
        {
            if (!this.ButtonInteraction)
                return;
            this.EventDown = true;
            this.AssemblyTransform.sizeDelta = new Vector2(this._OriginalSize.x * this.AdjustmentScale, this._OriginalSize.y * this.AdjustmentScale);
        }

        /// <summary>
        /// On pointer Up
        /// </summary>
        public virtual void OnPointerUp()
        {
            if (!this.ButtonInteraction)
                return;
            if (!this.EventDown)
                return;
            this.AssemblyTransform.sizeDelta = this._OriginalSize;
        }

        /// <summary>
        /// On Pointer Exit
        /// </summary>
        public virtual void OnPointerExit()
        {
            this.EventDown = false;
            this.AssemblyTransform.sizeDelta = this._OriginalSize;
        }
    }
}
