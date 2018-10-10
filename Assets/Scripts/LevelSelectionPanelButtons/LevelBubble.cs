using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;

namespace Game.UI
{
    public class LevelBubble : MonoBehaviour
    {
        /// <summary>
        /// How small the bubble can be
        /// </summary>
        public Vector2 MinSize = Vector2.zero;

        /// <summary>
        /// Minimum Text Size
        /// </summary>
        public int MinTextSize = 40;

        /// <summary>
        /// enlarging the level bubble
        /// </summary>
        public float Enlargement = 2.0f;

        /// <summary>
        /// Level display 
        /// </summary>
        public Text LevelNumberText;

        /// <summary>
        /// Locked icon for when disabled
        /// </summary>
        public Image LockIcon;

        /// <summary>
        /// Bubble icon transform for growing and shrinking
        /// </summary>
        public RectTransform BubbleRectTransform;

        /// <summary>
        /// Color of unlocked level
        /// </summary>
        public Color EnabledColor = Color.white;

        /// <summary>
        /// Color of locked level
        /// </summary>
        public Color DisabledColor = Color.gray;

        /// <summary>
        /// Check to see if pointer is down
        /// </summary>
        private bool _PointerDown = false;

        /// <summary>
        /// Check to see if this is being dragged, if dragged, all interactions go to LEvelScroller
        /// </summary>
        public bool IsDragging { get; set; }

        /// <summary>
        /// Check to see if the bubble is locked or not
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Configuration of this level
        /// </summary>
        private LevelConfig _LevelConfig;

        /// <summary>
        /// Level Config property
        /// </summary>
        public LevelConfig LevelConfig
        {
            get
            {
                return this._LevelConfig;
            }
            set
            {
                this._LevelConfig = value;
                this.LockIcon.enabled = false;
                if (this._LevelConfig.GameType == GameTypes.Tutorial)
                    this.LevelNumberText.text = "T";
                else
                    this.LevelNumberText.text = this.LevelID.ToString();
            }

        }

        /// <summary>
        /// Reference for when using the OnPointerDrag function
        /// </summary>
        public ScrollingLevelSelection _ScrollingLevelSection;

        /// <summary>
        /// Level ID of this bubble
        /// </summary>
        public int LevelID;

        /// <summary>
        /// Bubble Icon
        /// </summary>
        private Image _BubbleIcon;

        /// <summary>
        /// Awake this isntance
        /// </summary>
        private void Awake()
        {
            this._BubbleIcon = this.GetComponent<Image>();
        }

        /// <summary>
        /// On Pointer Down
        /// </summary>
        public void OnPointerDown()
        {
            this._ScrollingLevelSection.OnPointerDown();
            this._BubbleIcon.color = this.DisabledColor;
        }

        /// <summary>
        /// On Pointer Up
        /// </summary>
        public void OnPointerUp()
        {
            this._ScrollingLevelSection.OnPointerUp();
            if (!this.IsDragging)
            {
                this._ScrollingLevelSection.LevelPicker(this.LevelID);
                this._BubbleIcon.color = this.EnabledColor;
            }
            this.IsDragging = false;
        }

        /// <summary>
        /// On Pointer Drag
        /// </summary>
        public void OnPointerDrag()
        {
            this._ScrollingLevelSection.OnPointerDrag();
            this.IsDragging = true;
            this._BubbleIcon.color = this.EnabledColor;
        }

        /// <summary>
        /// Data input
        /// </summary>
        public void DataInput(LevelConfig config, int id, ScrollingLevelSelection levelSelector)
        {
            this.LevelID = id;
            this.LevelConfig = config;
            this._ScrollingLevelSection = levelSelector;
            //if (this._ScrollingLevelSection.CurrentNotch == this.LevelID)
            //    this.BubbleSizeAdjustment(1.0f);
            //else
            //    this.BubbleSizeAdjustment(0.0f);
        }

        /// <summary>
        /// Bubble size adjustment
        /// </summary>
        public void BubbleSizeAdjustment(float percentage)
        {
            this.BubbleRectTransform.sizeDelta = Vector2.Lerp(this.MinSize, this.MinSize * this.Enlargement, percentage);
            this.LevelNumberText.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(this.MinSize, this.MinSize * this.Enlargement, percentage);
            this.LevelNumberText.fontSize = (int)Mathf.Lerp(this.MinTextSize, this.MinTextSize * this.Enlargement, percentage);
        }
    }
}