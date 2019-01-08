using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;

namespace Game.UI
{
    public class SpeedAdjuster : MonoBehaviour
    {

        /// <summary>
        /// Slider component
        /// </summary>
        private Slider _Slider;

        /// <summary>
        /// Slider fill
        /// </summary>
        public Image SliderFill;

        /// <summary>
        /// Awake this instance
        /// </summary>
        private void Awake()
        {
            this._Slider = this.GetComponent<Slider>();
        }

        /// <summary>
        /// Start this instance
        /// </summary>
        private void Start()
        {
            Manager.Instance.SpeedTargetModifier = this._Slider.value * 10.0f;
            this.SliderFill.color = Manager.DisplayColor(this._Slider.value * 10.0f);
        }

        /// <summary>
        /// On Value Change
        /// </summary>
        public void OnValueChange()
        {
            float sliderValue = this._Slider.value * 10.0f;
            Manager.Instance.SpeedTargetModifier = sliderValue;
            Manager.Instance.SpeedMeter.AdjustSpeedIndicator(sliderValue);
            this.SliderFill.color = Manager.DisplayColor(sliderValue);
        }
    }
}