using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;

namespace Game.UI
{
    public class LevelSelectionPanel : Panel
    {

        /// <summary>
        /// Level Title Text
        /// </summary>
        public Text LevelTitleText;

        /// <summary>
        /// Objective Type Text
        /// </summary>
        public Text ObjectiveTypeText;

        /// <summary>
        /// Add this ocmponent in the editor
        /// </summary>
        public PlayButton PlayButton;

        /// <summary>
        /// Add this component in the editor
        /// </summary>
        public LootQuestButton LootQuestButton;

        /// <summary>
        /// Add this component in the editor
        /// </summary>
        public PracticeButton PracticeButton;

        /// <summary>
        /// Add this component in the editor
        /// </summary>
        public ScrollingLevelSelection ScrollingLevelSelection;

        /// <summary>
        /// Show Panel  
        /// </summary>
        public override void Show()
        {
            base.Show();
            this.ScrollingLevelSelection.CreateLevelSelector();
            this.ScrollingLevelSelection.FixedLevelSelect(HUDManager.Instance.SelectedLevelConfig.LevelID);
        }

        /// <summary>
        /// Hide Panel
        /// </summary>
        public override void Hide()
        {
            base.Hide();
        }

        /// <summary>
        /// When recieving input change all data
        /// </summary>
        public void DataInput(LevelConfig levelConfig)
        {
            switch (levelConfig.EnvironmentType)
            {
                case EnvironmentTypes.CityDay:
                    this.LevelTitleText.text = "City - Day";
                    break;
                case EnvironmentTypes.CityNight:
                    this.LevelTitleText.text = "City - Night";
                    break;
                case EnvironmentTypes.CitySunset:
                    this.LevelTitleText.text = "City - Sunset";
                    break;
                default:
                    this.LevelTitleText.text = "Invalid Map Type";
                    break;
            }
            switch (levelConfig.GameType)
            {
                case GameTypes.BeatTheClock:
                    this.ObjectiveTypeText.text = "Beat the Clock";
                    break;
                case GameTypes.Collector:
                    this.ObjectiveTypeText.text = "Collector";
                    break;
                case GameTypes.DefeatTheBoss:
                    this.ObjectiveTypeText.text = "Defeat the Boss";
                    break;
                case GameTypes.GetToTheEnd:
                    this.ObjectiveTypeText.text = "Get to the End";
                    break;
                case GameTypes.OutRun:
                    this.ObjectiveTypeText.text = "Outrun the Chaser";
                    break;
                case GameTypes.Tutorial:
                    this.ObjectiveTypeText.text = "Tutorial: The Basics";
                    break;
                default:
                    this.ObjectiveTypeText.text = "Invalid Game Type";
                    break;
            }
            this.PlayButton.ButtonToggle(true);
            this.PlayButton.ButtonEnabled(true);
            //TODO: Loot Quest button stuff
            this.LootQuestButton.ButtonToggle(false);
            this.PracticeButton.ButtonToggle(true);
            this.PracticeButton.ButtonEnabled(true);
        }

    }
}