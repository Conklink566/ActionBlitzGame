using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;

namespace Game.UI
{
    public class SpeedMeter : MonoBehaviour
    {
        /// <summary>
        /// Speed Rect Reference
        /// </summary>
        public RectTransform SpeedRectReference;

        /// <summary>
        /// Speed duck requirement
        /// </summary>
        public RectTransform SpeedDuckRequirement;

        /// <summary>
        /// Speed indicator
        /// </summary>
        public RectTransform SpeedIndicator;

        /// <summary>
        /// Speed fill
        /// </summary>
        public RectTransform SpeedFill;

        /// <summary>
        /// Speed boost text
        /// </summary>
        public Text SpeedBoostText;

        /// <summary>
        /// Slider
        /// </summary>
        public Slider Slider;

        /// <summary>
        /// Adjust display
        /// </summary>
        /// <param name="percentage"></param>
        public void AdjustDisplay(float percentage)
        {
            this.SpeedFill.sizeDelta = new Vector2(Mathf.Lerp(0.0f, this.SpeedRectReference.sizeDelta.x, percentage / 100.0f), this.SpeedFill.sizeDelta.y);
            this.SpeedBoostText.text = string.Format("{0:0}%", percentage);
            this.SpeedFill.gameObject.GetComponent<Image>().color = Manager.DisplayColor(percentage);
        }

        /// <summary>
        /// Adjust duck indicator
        /// </summary>
        /// <param name="percentage"></param>
        public void AdjustDuckIndicator()
        {
            this.SpeedDuckRequirement.anchoredPosition = new Vector2(Mathf.Lerp(0.0f, this.SpeedRectReference.sizeDelta.x, Manager.Instance.SpeedDuckRequirement / 100.0f), 
                                                                     this.SpeedDuckRequirement.anchoredPosition.y);
        }

        /// <summary>
        /// Adjust Speed Indicator
        /// </summary>
        /// <param name="percentage"></param>
        public void AdjustSpeedIndicator(float percentage)
        {
            this.SpeedIndicator.anchoredPosition = new Vector2(Mathf.Lerp(0.0f, this.SpeedRectReference.sizeDelta.x, percentage / 100.0f), this.SpeedIndicator.anchoredPosition.y);
            this.SpeedIndicator.gameObject.GetComponent<Image>().color = Manager.DisplayColor(percentage);
        }
    }
}