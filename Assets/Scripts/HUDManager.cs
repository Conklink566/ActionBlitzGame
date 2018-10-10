using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;

namespace Game.Interface
{


    public class HUDManager : MonoBehaviour
    {
        


        /// <summary>
        /// Panel Display History
        /// </summary>
        public List<Panel> PanelDisplayHistory = new List<Panel>();

        /// <summary>
        /// OverlayPanel
        /// </summary>
        public OverlayPanel OverlayPanel;

        /// <summary>
        /// Mainmenu Panel
        /// </summary>
        public MainPanel MainMenuPanel;

        /// <summary>
        /// Character Edit Panel
        /// </summary>
        public CharacterEditPanel CharacterEditPanel;

        /// <summary>
        /// Level Selection Panel
        /// </summary>
        public LevelSelectionPanel LevelSelectionPanel;

        /// <summary>
        /// Settings Panel
        /// </summary>
        public SettingsPanel SettingsPanel;

        /// <summary>
        /// Currently displayed Panel
        /// </summary>
        public Panel CurrentlyDisplayPanel;

        /// <summary>
        /// Width resolution reference
        /// </summary>
        public const float WidthResolution = 1920.0f;

        /// <summary>
        /// Selected Level Config from save file
        /// </summary>
        public LevelConfig SelectedLevelConfig;

        /// <summary>
        /// Selected Character Config from save file
        /// </summary>
        public CharacterConfig SelectedCharacterConfig;

        /// <summary>
        /// Add this component in the editor
        /// </summary>
        public BackgroundMovement BackgroundMovement;

        /// <summary>
        /// Create this HUDManager globally
        /// </summary>
        public static HUDManager Instance;

        /// <summary>
        /// Awake this instance
        /// </summary>
        private void Awake()
        {
            HUDManager.Instance = this;
        }

        /// <summary>
        /// Start this instance
        /// </summary>
        private void Start()
        {
            //Get level config from Save File
            FileConfigHandler.Load();
            this.SelectedLevelConfig = FileConfigHandler.Instance.UserConfig.LevelConfig;
            this.SelectedCharacterConfig = FileConfigHandler.Instance.UserConfig.CharacterConfig.Copy();
            this.AddPanelToList(this.OverlayPanel, false);
            this.AddPanelToList(this.MainMenuPanel, true);
            this.BackgroundMovement.ChangeEnvironment(AssetFactory.Instance.BackgroundType(this.SelectedLevelConfig.EnvironmentType));
        }

        /// <summary>
        /// Add and display the next panel
        /// </summary>
        public void AddPanelToList(Panel panelSelected, bool addToHistory)
        {
            if (addToHistory)
            {
                this.PanelDisplayHistory.Add(panelSelected);
                if (this.CurrentlyDisplayPanel != null && this.CurrentlyDisplayPanel != this.OverlayPanel)
                    this.CurrentlyDisplayPanel.Hide();
            }
            panelSelected.Show();
            this.CurrentlyDisplayPanel = panelSelected;
            this.OverlayPanel.CheckHistory();
        }

        /// <summary>
        /// Remove panel from list and display the previous panel
        /// </summary>
        public void RemovePanelToList(bool removeFromHistory)
        {
            if (this.CurrentlyDisplayPanel != null && this.CurrentlyDisplayPanel != this.OverlayPanel)
                this.CurrentlyDisplayPanel.Hide();
            if (removeFromHistory)
                this.PanelDisplayHistory.RemoveAt(this.PanelDisplayHistory.Count - 1);
            this.CurrentlyDisplayPanel = this.PanelDisplayHistory[this.PanelDisplayHistory.Count - 1];
            this.CurrentlyDisplayPanel.Show();
            this.OverlayPanel.CheckHistory();
        }
    }
}