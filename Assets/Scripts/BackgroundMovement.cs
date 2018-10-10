using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;

namespace Game.UI
{
    public class BackgroundMovement : MonoBehaviour
    {
        /// <summary>
        /// background far object Current
        /// </summary>
        public RectTransform[] BackgroundCurrent;

        /// <summary>
        /// background far object Next
        /// </summary>
        public RectTransform[] BackgroundNext;

        /// <summary>
        /// Background environment color
        /// </summary>
        public Image BackgroundImage;

        /// <summary>
        /// Background light source in the sky
        /// </summary>
        public Image BackgroundLightSource;

        /// <summary>
        /// background far object Current
        /// </summary>
        public RectTransform[] FadingBackgroundCurrent;

        /// <summary>
        /// background far object Next
        /// </summary>
        public RectTransform[] FadingBackgroundNext;

        /// <summary>
        /// Background environment color
        /// </summary>
        public Image FadingBackgroundImage;

        /// <summary>
        /// Background light source in the sky
        /// </summary>
        public Image FadingBackgroundLightSource;

        /// <summary>
        /// New Background input
        /// </summary>
        private Sprite[] _NewBackgroundInput;

        /// <summary>
        /// When getting a new background input, fadeOut option will be available
        /// </summary>
        public Sprite[] NewBackgroundInput
        {
            get
            {
                if (this._NewBackgroundInput == null)
                    this._NewBackgroundInput = AssetFactory.Instance.BackgroundType(HUDManager.Instance.SelectedLevelConfig.EnvironmentType);
                return this._NewBackgroundInput;
            }
            set
            {
                if(this.IsFadingIn)
                {
                    this.ChangeEnvironment(this._NewBackgroundInput);
                }
                this.IsFadingIn = true;
                this._FadeInOutValue = 0.0f;
                this._NewBackgroundInput = value;
                for (int i = 0; i < this.FadingBackgroundCurrent.Length; i++)
                {
                    this.FadingBackgroundCurrent[i].GetComponent<Image>().sprite = this._NewBackgroundInput[i];
                    this.FadingBackgroundNext[i].GetComponent<Image>().sprite = this._NewBackgroundInput[i];
                }
                this.FadingBackgroundImage.sprite = this._NewBackgroundInput[3];
                this.FadingBackgroundLightSource.sprite = this._NewBackgroundInput[4];
                //this._ReceivedNewBackground = true;
            }
        }

        /// <summary>
        /// Check to see if the input has been received, and will be waiting for when the FadeIn has finished before adding in new input
        /// </summary>
        //private bool _ReceivedNewBackground = false;

        /// <summary>
        /// Used for calculations for tileable background
        /// </summary>
        private float[] _PivotX;

        /// <summary>
        /// Amount of times that loops the background for tiling
        /// </summary>
        private int[] _LoopAmounts;

        /// <summary>
        /// Background Adjustment
        /// </summary>
        public float BackgroundAdjustment;

        /// <summary>
        /// Testing usage
        /// </summary>
        public float Adjuster;

        /// <summary>
        /// Fade in and out
        /// </summary>
        public Image FadeInOutPanel;

        /// <summary>
        /// If we are not fading in, fade out
        /// </summary>
        public bool IsFadingIn { get; set; }

        /// <summary>
        /// 0 = fade out, 1 = fade in
        /// </summary>
        private float _FadeInOutValue;

        /// <summary>
        /// How fast is the fade supposed to be
        /// </summary>
        public float FadeInOutDurationSpeed;

        /// <summary>
        /// Fade in color
        /// </summary>
        public Color FadeInColor;

        /// <summary>
        /// Fade out color
        /// </summary>
        public Color FadeOutColor;

        private void Start()
        {
            this.ResetLists();
        }

        /// <summary>
        /// Update every frame
        /// </summary>
        private void Update()
        {
            this.UpdateBackground(this.Adjuster * Time.deltaTime);
            if (!this.IsFadingIn)
                return;
            this.SpriteTransition();
            //if (this.IsFadingIn && this._FadeInOutValue >= 1.0f && !this._ReceivedNewBackground || 
            //    !this.IsFadingIn && this._FadeInOutValue <= 0.0f && !this._ReceivedNewBackground)
            //    return;
            //this.FadeInOut();
        }

        /// <summary>
        /// value addition
        /// </summary>
        /// <param name="valueAddition"></param>
        public void UpdateBackground(float valueAddition)
        {
            for(int i = 0; i < this.BackgroundCurrent.Length; i++)
            {
                this._PivotX[i] -= valueAddition;
                if(-this.BackgroundCurrent[i].sizeDelta.x * (this._LoopAmounts[i] + 1) > this._PivotX[i] / Mathf.Pow(this.BackgroundAdjustment, i + 1))
                {
                    this._LoopAmounts[i]++;
                }
                this.BackgroundCurrent[i].anchoredPosition = new Vector2((this._PivotX[i] / Mathf.Pow(this.BackgroundAdjustment, i + 1) + (this._LoopAmounts[i] * this.BackgroundCurrent[i].sizeDelta.x)),
                                                                         this.BackgroundCurrent[i].anchoredPosition.y);
                this.BackgroundNext[i].anchoredPosition = new Vector2((this._PivotX[i] / Mathf.Pow(this.BackgroundAdjustment, i + 1) + ((this._LoopAmounts[i] + 1) * this.BackgroundNext[i].sizeDelta.x)),
                                                                         this.BackgroundNext[i].anchoredPosition.y);
                this.FadingBackgroundCurrent[i].anchoredPosition = new Vector2((this._PivotX[i] / Mathf.Pow(this.BackgroundAdjustment, i + 1) + (this._LoopAmounts[i] * this.BackgroundCurrent[i].sizeDelta.x)),
                                                                         this.BackgroundCurrent[i].anchoredPosition.y);
                this.FadingBackgroundNext[i].anchoredPosition = new Vector2((this._PivotX[i] / Mathf.Pow(this.BackgroundAdjustment, i + 1) + ((this._LoopAmounts[i] + 1) * this.BackgroundNext[i].sizeDelta.x)),
                                                                         this.BackgroundNext[i].anchoredPosition.y);

            }
        }

        /// <summary>
        /// Reset Lists
        /// </summary>
        public void ResetLists()
        {
            this._PivotX = new float[this.BackgroundCurrent.Length];
            this._LoopAmounts = new int[this.BackgroundCurrent.Length];
            for(int i = 0; i < this.FadingBackgroundCurrent.Length; i++)
            {
                this.FadingBackgroundCurrent[i].GetComponent<Image>().color = Color.clear;
                this.FadingBackgroundNext[i].GetComponent<Image>().color = Color.clear;
            }
            this.FadingBackgroundImage.color = Color.clear;
            this.FadingBackgroundLightSource.color = Color.clear;
        }

        /// <summary>
        /// Fade in and out through the bool parameter
        /// </summary>
        //public void FadeInOut()
        //{
        //    if(this.IsFadingIn)
        //        this._FadeInOutValue += Time.deltaTime / this.FadeInOutDurationSpeed;
        //    else
        //        this._FadeInOutValue -= Time.deltaTime / this.FadeInOutDurationSpeed;
        //    this.FadeInOutPanel.color = Color.Lerp(this.FadeOutColor, this.FadeInColor, this._FadeInOutValue);
        //    if(this._ReceivedNewBackground && this._FadeInOutValue >= 1.0f)
        //    {
        //        this.ChangeEnvironment(this.NewBackgroundInput);
        //    }
        //}

        /// <summary>
        /// Transition between two backgrounds
        /// </summary>
        public void SpriteTransition()
        {
            this._FadeInOutValue += Time.deltaTime / this.FadeInOutDurationSpeed;
            for (int i = 0; i < this.FadingBackgroundCurrent.Length; i++)
            {
                this.FadingBackgroundCurrent[i].GetComponent<Image>().color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), Color.white, this._FadeInOutValue);
                this.FadingBackgroundNext[i].GetComponent<Image>().color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), Color.white, this._FadeInOutValue);
            }
            this.FadingBackgroundImage.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), Color.white, this._FadeInOutValue);
            this.FadingBackgroundLightSource.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), Color.white, this._FadeInOutValue);
            if (this._FadeInOutValue >= 1.0f)
            {
                this.IsFadingIn = false;
                for (int i = 0; i < this.FadingBackgroundCurrent.Length; i++)
                {
                    this.FadingBackgroundCurrent[i].GetComponent<Image>().color = Color.clear;
                    this.FadingBackgroundNext[i].GetComponent<Image>().color = Color.clear;
                }
                this.FadingBackgroundImage.color = Color.clear;
                this.FadingBackgroundLightSource.color = Color.clear;
                this.ChangeEnvironment(this.NewBackgroundInput);
            }
        }

        /// <summary>
        /// Changing background environment level
        /// </summary>
        public void ChangeEnvironment(Sprite[] backgroundEnvironment)
        {
            for(int i = 0; i < this.BackgroundCurrent.Length; i++)
            {
                this.BackgroundCurrent[i].GetComponent<Image>().sprite = backgroundEnvironment[i];
                this.BackgroundNext[i].GetComponent<Image>().sprite = backgroundEnvironment[i];
            }
            this.BackgroundImage.sprite = backgroundEnvironment[3];
            this.BackgroundLightSource.sprite = backgroundEnvironment[4];
            //this.ResetLists();
            this.IsFadingIn = false;
            //this._ReceivedNewBackground = false;
        }
    }
}