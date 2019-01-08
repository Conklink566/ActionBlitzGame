using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        /// <summary>
        /// Reference for 0% and 100% of the loadingBarFill
        /// </summary>
        public RectTransform LoadingBarRef;

        /// <summary>
        /// Fills in the bar to show if it is 0% to 100%
        /// </summary>
        public RectTransform LoadingBarFill;

        /// <summary>
        /// Display of percentage of the Loading bar
        /// </summary>
        public Text LoadingBarText;

        /// <summary>
        /// Background movement
        /// </summary>
        public BackgroundMovement BackgroundMovement;

        /// <summary>
        /// Start this instance
        /// </summary>
        private void Start()
        {
            LevelConfig config = FileConfigHandler.Instance.UserConfig.LevelConfig;
            this.LoadingBarFill.sizeDelta = new Vector2(0.0f, this.LoadingBarFill.sizeDelta.y);
            this.LoadingBarText.text = string.Format("Loading... {0:0.0}%", 0.0f);
            this.BackgroundMovement.ChangeEnvironment(AssetFactory.Instance.BackgroundType(config.EnvironmentType));
            StartCoroutine(this.LoadMainLevelScene());
        }

        /// <summary>
        /// Load Main Level Scene
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadMainLevelScene()
        {
            yield return new WaitForSeconds(1.0f);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)SceneNames.MainLevel);
            asyncLoad.allowSceneActivation = false;
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                this.LoadingBarFill.sizeDelta = new Vector2(asyncLoad.progress * this.LoadingBarRef.sizeDelta.x, this.LoadingBarRef.sizeDelta.y);
                this.LoadingBarText.text = string.Format("Loading... {0:0.0}%", asyncLoad.progress * 100.0f);
                if(asyncLoad.progress >= 0.9f)
                {
                    this.LoadingBarFill.sizeDelta = new Vector2(this.LoadingBarRef.sizeDelta.x, this.LoadingBarRef.sizeDelta.y);
                    this.LoadingBarText.text = string.Format("Loading... {0:0.0}%", 100.0f);
                    yield return new WaitForSeconds(1.0f);
                        asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}