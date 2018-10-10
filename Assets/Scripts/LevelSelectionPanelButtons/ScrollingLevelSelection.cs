using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;

namespace Game.UI
{
    public class ScrollingLevelSelection : MonoBehaviour
    {
        /// <summary>
        /// Level Select Transform
        /// </summary>
        private RectTransform _LevelSelectTransform;

        /// <summary>
        /// Previous Mouse Position for drag speed
        /// </summary>
        private Vector2 _PreviousMousePosition;

        /// <summary>
        /// How fast the swipping is
        /// </summary>
        private float _SpeedDistanceDifference = 0.0f;

        /// <summary>
        /// Time drag after release for draging
        /// </summary>
        public float TimeDragAfterRelease = 0.0f;

        /// <summary>
        /// Amount of time it takes to do a full adjustment without adjustment percentage
        /// </summary>
        public float TimeForAdjusting = 0.0f;

        /// <summary>
        /// Time till the scrolling drag stops
        /// </summary>
        public float CurrentScrollingTime = 0.0f;

        /// <summary>
        /// Time till the scroller aligned
        /// </summary>
        public float CurrentAdjustmentTime = 0.0f;

        /// <summary>
        /// pointer down
        /// </summary>
        private bool _PointerDown = false;

        /// <summary>
        /// Distance between each level on the scroller
        /// </summary>
        public float LevelGap = 0.0f;

        /// <summary>
        /// Each notch referenced with the levelGap, to see how far apart each LevelBubble has to be
        /// </summary>
        public int CurrentNotch = 0;

        /// <summary>
        /// Prefab Level Bubble
        /// </summary>
        public GameObject LevelBubble;

        /// <summary>
        /// LevelBar that all the bubbles connect to
        /// </summary>
        public GameObject LevelBar;

        /// <summary>
        /// Current Scorlling Speed
        /// </summary>
        public float _CurrentScrollingSpeed = 0.0f;

        /// <summary>
        /// Used to control and limit the movement of the scroller
        /// </summary>
        private float _CenterIndex = 0.0f;

        /// <summary>
        /// Set Adjustment values after the scroll dragging has stopped
        /// </summary>
        private bool _SetAdjustmentValues = false;

        /// <summary>
        /// Set this value after the dragging has stopped
        /// </summary>
        public float _AdjustmentPercentageRemainder = 0.0f;

        /// <summary>
        /// PreviousIndex Reference to CenterIndex
        /// </summary>
        public float _PreviousIndex = 0.0f;

        /// <summary>
        /// Check to see if there has been a complete cycle of the level selector
        /// </summary>
        private bool CycleCheck = false;

        /// <summary>
        /// To see if there has been a clicked input, which moves the scroller to the clicked area
        /// </summary>
        private bool _LevelClickInput = false;

        /// <summary>
        /// Time for how long the transition is between each click
        /// </summary>
        public float CurrentLevelClickerTime = 0.0f;

        /// <summary>
        /// Level Selected
        /// </summary>
        private int _LevelSelected = 0;

        /// <summary>
        /// Add this component in the editor
        /// </summary>
        public LevelSelectionPanel LevelSelectionPanel;



        /// <summary>
        /// Level Selected
        /// </summary>
        public int LevelSelected
        {
            get
            {
                return this._LevelSelected;
            }
            set
            {
                this._LevelSelected = value;
                //Changes to new level Config through the levelSelectionPanel
                HUDManager.Instance.SelectedLevelConfig = AssetFactory.Instance.LevelConfigList[value];
                if(AssetFactory.Instance.BackgroundType(HUDManager.Instance.SelectedLevelConfig.EnvironmentType) != HUDManager.Instance.BackgroundMovement.NewBackgroundInput)
                    HUDManager.Instance.BackgroundMovement.NewBackgroundInput = AssetFactory.Instance.BackgroundType(HUDManager.Instance.SelectedLevelConfig.EnvironmentType);
                this.LevelSelectionPanel.DataInput(HUDManager.Instance.SelectedLevelConfig);
            }
        }

        /// <summary>
        /// Center Index property that interacts with the LevelBubbles
        /// </summary>
        public float CenterIndex
        {
            get
            {
                return this._CenterIndex;
            }
            private set
            {
                this._CenterIndex = value;
                //Adjust for new notches
                int correctLevelNotch = (int)((-this.CenterIndex + (this.LevelGap / 2)) / this.LevelGap);
                if(correctLevelNotch < 0)
                {
                    correctLevelNotch = 0;
                }
                else if(correctLevelNotch >= this.LevelBubbleSpawnedList.Length)
                {
                    correctLevelNotch = this.LevelBubbleSpawnedList.Length - 1;
                }
                int loopBreak = 0;
                while(this.CurrentNotch != correctLevelNotch)
                {
                    if(loopBreak > 500)
                    {
                        throw new System.Exception("Loop is infinite. Error");
                    }
                    this.LevelBubbleSpawnedList[this.CurrentNotch].BubbleSizeAdjustment(0.0f);
                    if (this.CurrentNotch < correctLevelNotch)
                    {
                        this.CurrentNotch++;
                    }
                    else
                    {
                        this.CurrentNotch--;
                    }
                    if(this.CurrentNotch != this.LevelSelected)
                    {
                        this.LevelSelectionPanel.PlayButton.ButtonToggle(false);
                        this.LevelSelectionPanel.LootQuestButton.ButtonToggle(false);
                        this.LevelSelectionPanel.PracticeButton.ButtonToggle(false);
                        this.LevelSelectionPanel.ObjectiveTypeText.text = string.Empty;
                        if(AssetFactory.Instance.LevelConfigList[this.CurrentNotch].EnvironmentType != HUDManager.Instance.SelectedLevelConfig.EnvironmentType)
                        {
                            //HUDManager.Instance.BackgroundMovement.IsFadingIn = true;
                            this.LevelSelectionPanel.LevelTitleText.text = string.Empty;
                        }
                    }
                    loopBreak++;
                }
                //Adjust bubble size by percentage
                float remainder = -this.CenterIndex %  this.LevelGap;
                if (remainder == 0.0f)
                {
                    this.LevelBubbleSpawnedList[this.CurrentNotch].BubbleSizeAdjustment(1.0f);
                }
                else
                {
                    float adjust = 1.0f - (remainder / (this.LevelGap / 2.0f));
                    this.LevelBubbleSpawnedList[this.CurrentNotch].BubbleSizeAdjustment(Mathf.Abs(adjust));
                    this.LevelSelectionPanel.PlayButton.ButtonEnabled(false);
                    this.LevelSelectionPanel.PracticeButton.ButtonEnabled(false);
                    this.LevelSelectionPanel.LootQuestButton.ButtonEnabled(false);
                }
            }
        }


        /// <summary>
        /// x = the min, y = the max, for the limit of where the CenterIndex can go.
        /// </summary>
        private Vector2 _IndexBoundaries;

        /// <summary>
        /// List of bubbles created and stored in this list
        /// </summary>
        public LevelBubble[] LevelBubbleSpawnedList;

        /// <summary>
        /// Check to see if scroller has been created
        /// </summary>
        private bool ScrollerCreated = false;

        // Update is called once per frame
        private void Update()
        {
            if (this.CycleCheck)
                return;
            if(this._LevelClickInput)
            {
                this.CurrentLevelClickerTime += (Time.deltaTime / this.TimeForAdjusting);
                float position = Mathf.Lerp(this._PreviousIndex, -this.LevelGap * this.LevelSelected, this.CurrentLevelClickerTime);
                this.CenterIndex = position;
                this._LevelSelectTransform.anchoredPosition = new Vector2(position, this._LevelSelectTransform.anchoredPosition.y);
                if (this.CurrentLevelClickerTime >= 1.0f)
                    this._LevelClickInput = false;
                return;
            }
            //For dragging effect for when the cursor is released
            this.CurrentScrollingTime += (Time.deltaTime / this.TimeDragAfterRelease);
            this._CurrentScrollingSpeed = Mathf.Lerp(this._SpeedDistanceDifference, 0.0f, this.CurrentScrollingTime);
            if (this._PointerDown)
                return;
            if (this._CurrentScrollingSpeed != 0.0f)
            {
                this.ScrollDrag();
                return;
            }
            if (this.CenterIndex != this.CurrentNotch * -this.LevelGap)
            {
                this.AdjustToNotch();
                return;
            }
            //if (this.CurrentNotch != this.LevelSelected)
            //    this.LevelSelected = this.CurrentNotch;
            this.LevelSelected = this.CurrentNotch;

            //New Level Input sent

            this.CycleCheck = true;
        }

        /// <summary>
        /// On Pointer Down
        /// </summary>
        public void OnPointerDown()
        {
            this._PreviousMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            this._SetAdjustmentValues = false;
            this._PointerDown = true;
        }

        /// <summary>
        /// On Pointer Up
        /// </summary>
        public void OnPointerUp()
        {
            this._PointerDown = false;
        }

        /// <summary>
        /// On Pointer Drag
        /// </summary>
        public void OnPointerDrag()
        {
            this.CycleCheck = false;
            this.CurrentScrollingTime = 0.0f;
            Vector2 position1 = new Vector2(this._PreviousMousePosition.x, 0.0f);
            Vector2 position2 = new Vector2(Camera.main.ScreenToViewportPoint(Input.mousePosition).x, 0.0f);
            this._SpeedDistanceDifference = Vector2.Distance(position1, position2) * HUDManager.WidthResolution;
            if (position1.x > position2.x)
            {
                this._SpeedDistanceDifference *= -1.0f;
            }
            this._PreviousMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            this.CenterIndex = this._LevelSelectTransform.anchoredPosition.x + this._SpeedDistanceDifference;
            if (this.CenterIndex < this._IndexBoundaries.x)
            {
                this.CenterIndex = this._IndexBoundaries.x;
                this._CurrentScrollingSpeed = 0.0f;
            }
            else if (this.CenterIndex > this._IndexBoundaries.y)
            {
                this.CenterIndex = this._IndexBoundaries.y;
                this._CurrentScrollingSpeed = 0.0f;
            }
            this._LevelSelectTransform.anchoredPosition = new Vector2(this.CenterIndex, this._LevelSelectTransform.anchoredPosition.y);
        }

        /// <summary>
        /// Create Level Selector, controlled by the levelSelectionPanel
        /// </summary>
        public void CreateLevelSelector()
        {
            if (this.ScrollerCreated)
                return;
            this.ScrollerCreated = true;
            //Create and/or align level selection
            this.LevelBubbleSpawnedList = new LevelBubble[AssetFactory.Instance.LevelConfigList.Length];
            GameObject barObj = (GameObject)Instantiate(this.LevelBar);
            barObj.transform.SetParent(this.transform);
            barObj.transform.localScale = Vector3.one;
            this._LevelSelectTransform = barObj.GetComponent<RectTransform>();
            this._LevelSelectTransform.anchoredPosition3D = Vector3.zero;
            this._LevelSelectTransform.sizeDelta = new Vector2((this.LevelBubbleSpawnedList.Length - 1) * this.LevelGap, this._LevelSelectTransform.sizeDelta.y);
            for(int i = 0; i < this.LevelBubbleSpawnedList.Length; i++)
            {
                GameObject bubbleObj = (GameObject)Instantiate(this.LevelBubble);
                bubbleObj.transform.SetParent(this._LevelSelectTransform);
                bubbleObj.transform.localScale = Vector3.one;
                LevelBubble levelBubble = bubbleObj.GetComponent<LevelBubble>();
                levelBubble.BubbleRectTransform.anchoredPosition3D = new Vector3(i * this.LevelGap, 0.0f, 0.0f);
                this.LevelBubbleSpawnedList[i] = levelBubble;
                levelBubble.DataInput(AssetFactory.Instance.LevelConfigList[i], i, this);
            }
            this._IndexBoundaries = new Vector2((this.LevelBubbleSpawnedList.Length - 1) * -this.LevelGap, 0.0f);
            this.CycleCheck = false;
        }

        /// <summary>
        /// Adjust to notch after scroll drag stops
        /// </summary>
        private void AdjustToNotch()
        {
            //Start adjusting to the notches
            this.CenterIndex = this._LevelSelectTransform.anchoredPosition.x;
            if (!this._SetAdjustmentValues)
            {
                float remainder = -this.CenterIndex % (this.LevelGap / 2.0f);
                this._AdjustmentPercentageRemainder = remainder / (this.LevelGap / 2.0f);
                if (this.CenterIndex < (this.CurrentNotch * -this.LevelGap))
                    this._AdjustmentPercentageRemainder = 1.0f - this._AdjustmentPercentageRemainder;
                this.CurrentAdjustmentTime = 0.0f;
                this._SetAdjustmentValues = true;
            }
            this.CurrentAdjustmentTime += (Time.deltaTime / this.TimeForAdjusting);
            float calculation = 2.0f * (this.CenterIndex < (this.CurrentNotch * -this.LevelGap) ? -1.0f : 1.0f);
            float position = (this.CurrentNotch * -this.LevelGap) + (this.LevelGap / calculation);
            this.CenterIndex = Mathf.Lerp(position, -this.CurrentNotch * this.LevelGap, this._AdjustmentPercentageRemainder + this.CurrentAdjustmentTime);
            this._LevelSelectTransform.anchoredPosition = new Vector2(this.CenterIndex, this._LevelSelectTransform.anchoredPosition.y);
        }

        /// <summary>
        /// Scroll Drag after pointer release
        /// </summary>
        private void ScrollDrag()
        {
            //Start scroll dragging
            this.CenterIndex = this._LevelSelectTransform.anchoredPosition.x + this._CurrentScrollingSpeed;
            if (this.CenterIndex < this._IndexBoundaries.x)
            {
                this.CenterIndex = this._IndexBoundaries.x;
                this._CurrentScrollingSpeed = 0.0f;
            }
            else if (this.CenterIndex > this._IndexBoundaries.y)
            {
                this.CenterIndex = this._IndexBoundaries.y;
                this._CurrentScrollingSpeed = 0.0f;
            }
            this._LevelSelectTransform.anchoredPosition = new Vector2(this.CenterIndex, this._LevelSelectTransform.anchoredPosition.y);

        }

        /// <summary>
        /// Level selected by clicking the level bubble
        /// </summary>
        /// <param name="levelSelected"></param>
        public void LevelPicker(int levelSelected)
        {
            this.LevelSelected = levelSelected;
            this._LevelClickInput = true;
            this.CurrentLevelClickerTime = 0.0f;
            this._SpeedDistanceDifference = 0.0f;
            this._PreviousIndex = this.CenterIndex;
            this.CycleCheck = false;
        }

        /// <summary>
        /// Forces the scroller to go to the level bubble instantly, controlled by the level Selector
        /// </summary>
        public void FixedLevelSelect(int levelSelected)
        {
            this._LevelSelected = levelSelected;
            this.CurrentNotch = levelSelected;
            this.CenterIndex = this.CurrentNotch * -this.LevelGap;
            this._LevelSelectTransform.anchoredPosition = new Vector2(this.CenterIndex, this._LevelSelectTransform.anchoredPosition.y);
        }
    }
}