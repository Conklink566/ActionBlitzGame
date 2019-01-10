using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class ContinueButton : EventButton
    {
        /// <summary>
        /// ?Button Down
        /// </summary>
        private bool _ButtonDown = false;

        /// <summary>
        /// Enabled Color (Usually White)
        /// </summary>
        public Color EnabledColor = Color.white;

        /// <summary>
        /// Disabled color
        /// </summary>
        public Color DisabledColor = Color.gray;

        /// <summary>
        /// For enabling and disabling purposes
        /// </summary>
        public Image FrameIcon;

        /// <summary>
        /// For enabling and disabling purposes
        /// </summary>
        public Text ButtonText;

        /// <summary>
        /// Awake this instance
        /// </summary>
        public override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// On Pointer Down
        /// </summary>
        public override void OnPointerDown()
        {
            base.OnPointerDown();
            this._ButtonDown = true;
        }

        /// <summary>
        /// On pointer Up
        /// </summary>
        public override void OnPointerUp()
        {
            base.OnPointerUp();
            if (!this._ButtonDown)
                return;
            this._ButtonDown = false;
            //Save config data
            FileConfigHandler.Instance.UserConfig.LevelConfig = HUDManager.Instance.SelectedLevelConfig;
            FileConfigHandler.Save();
            //Load Main Scene
            AudioManager.Instance.AudioSource.Stop();
            SceneManager.LoadScene((int)SceneNames.LoadingAssets);
        }

        /// <summary>
        /// On Pointer Exit
        /// </summary>
        public override void OnPointerExit()
        {
            base.OnPointerExit();
            this._ButtonDown = false;
        }

        /// <summary>
        /// Toggle the visability of the button
        /// </summary>
        public void ButtonToggle(bool toggle)
        {
            this.gameObject.SetActive(toggle);
        }

        /// <summary>
        /// Disable button interactions
        /// </summary>
        public void ButtonEnabled(bool toggle)
        {
            this.ButtonInteraction = toggle;
            this.FrameIcon.color = toggle ? this.EnabledColor : this.DisabledColor;
            this.ButtonText.color = toggle ? this.EnabledColor : this.DisabledColor;
        }
    }
}