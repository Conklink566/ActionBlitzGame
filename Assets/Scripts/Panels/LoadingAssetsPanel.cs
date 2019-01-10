using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class LoadingAssetsPanel : MonoBehaviour
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
        /// Start this instance
        /// </summary>
        private void Start()
        {
            this.LoadingBarFill.sizeDelta = new Vector2(0.0f, this.LoadingBarFill.sizeDelta.y);
            this.LoadingBarText.text = string.Format("Loading... {0:0.0}%", 0.0f);
            StartCoroutine(this.LoadAssets());
        }

        /// <summary>
        /// Load Main Level Scene
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadAssets()
        {
            Resources.LoadAll("Art");
            Resources.LoadAll("Animation");

            AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync((int)SceneNames.MainMenu);
            asyncLoadScene.allowSceneActivation = false;
            // Wait until the asynchronous scene fully loads
            while (!asyncLoadScene.isDone)
            {
                this.LoadingBarFill.sizeDelta = new Vector2(asyncLoadScene.progress * this.LoadingBarRef.sizeDelta.x, this.LoadingBarRef.sizeDelta.y);
                this.LoadingBarText.text = string.Format("Loading... {0:0.0}%", asyncLoadScene.progress);
                if (asyncLoadScene.progress >= 0.9f)
                {
                    this.LoadingBarFill.sizeDelta = new Vector2(this.LoadingBarRef.sizeDelta.x, this.LoadingBarRef.sizeDelta.y);
                    this.LoadingBarText.text = string.Format("Loading... {0:0.0}%", 100.0f);
                    yield return new WaitForSeconds(1.0f);
                    asyncLoadScene.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}