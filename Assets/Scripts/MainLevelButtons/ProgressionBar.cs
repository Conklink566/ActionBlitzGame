using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;

namespace Game.UI
{
    public class ProgressionBar : MonoBehaviour
    {
        /// <summary>
        /// Bar Line UI
        /// </summary>
        public RectTransform BarLine;

        /// <summary>
        /// Progression mark on the line
        /// </summary>
        public RectTransform MarkIcon;

        /// <summary>
        /// x = min, y = max
        /// </summary>
        private Vector2 ProgressionNumbers;

        /// <summary>
        /// Progression Value
        /// </summary>
        private float _ProgressionValue = 0.0f;

        /// <summary>
        /// Awake this instance
        /// </summary>
        private void Awake()
        {
            if (FileConfigHandler.Instance.UserConfig.LevelConfig.LevelLength == 0.0f)
                this.gameObject.SetActive(false);
            else
                this.gameObject.SetActive(true);
            this.ProgressionNumbers = new Vector2(0.0f, this.BarLine.sizeDelta.x);
            this.MarkIcon.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Adjusting Marker
        /// </summary>
        public void AdjustMarker(float newPosition)
        {
            if (newPosition <= 0.0f || 
                newPosition > FileConfigHandler.Instance.UserConfig.LevelConfig.LevelLength)
                return;
            this._ProgressionValue = newPosition / FileConfigHandler.Instance.UserConfig.LevelConfig.LevelLength;
            this.MarkIcon.anchoredPosition = new Vector2(this._ProgressionValue * this.ProgressionNumbers.y, 0.0f);
        }

        /// <summary>
        /// Resets the progression bar
        /// </summary>
        public void Reset()
        {
            this._ProgressionValue = 0.0f;
            this.MarkIcon.anchoredPosition = new Vector2(this._ProgressionValue * this.ProgressionNumbers.y, 0.0f);
        }
    }
}