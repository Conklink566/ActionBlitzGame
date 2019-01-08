using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Interface
{


    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// Create this instance
        /// </summary>
        public static AudioManager Instance;

        /// <summary>
        /// Get this component
        /// </summary>
        private AudioSource _AudioSource;

        /// <summary>
        /// List of Main Menu Clips
        /// </summary>
        [SerializeField]
        private AudioClip[] _MainMenuClips;

        /// <summary>
        /// List of Main Level Clips
        /// </summary>
        [SerializeField]
        private AudioClip[] _MainLevelClips;

        /// <summary>
        /// Current Scene Display
        /// </summary>
        public string CurrentSceneDisplay;

        /// <summary>
        /// Time to Start Audio
        /// </summary>
        public float TimerToStartAudio = 0.0f;

        /// <summary>
        /// Selected Environment Type
        /// </summary>
        private EnvironmentTypes _SelectedType;

        /// <summary>
        /// Current timer required to start the song
        /// </summary>
        private float _CurrentTimer = 0.0f;

        /// <summary>
        /// Awake this instance
        /// </summary>
        private void Awake()
        {
            this._AudioSource = this.GetComponent<AudioSource>();
            if (AudioManager.Instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                AudioManager.Instance = this;
            }
            else if (AudioManager.Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Set AudioClips
        /// </summary>
        public void SetAudioClip(EnvironmentTypes type)
        {
            this._AudioSource.Stop();
            switch(type)
            {
                case EnvironmentTypes.CityDay:
                    if(SceneManager.GetSceneByBuildIndex(0).name == this.CurrentSceneDisplay && this._MainMenuClips.Length != 0)
                        this._AudioSource.clip = this._MainMenuClips[0];
                    else if (SceneManager.GetSceneByBuildIndex(2).name == this.CurrentSceneDisplay && this._MainLevelClips.Length != 0)
                        this._AudioSource.clip = this._MainLevelClips[0];
                    break;
                case EnvironmentTypes.CityNight:
                    if (SceneManager.GetSceneByBuildIndex(0).name == this.CurrentSceneDisplay && this._MainMenuClips.Length != 0)
                        this._AudioSource.clip = this._MainMenuClips[1];
                    else if (SceneManager.GetSceneByBuildIndex(2).name == this.CurrentSceneDisplay && this._MainLevelClips.Length != 0)
                        this._AudioSource.clip = this._MainLevelClips[1];
                    break;
                case EnvironmentTypes.CitySunset:
                    if (SceneManager.GetSceneByBuildIndex(0).name == this.CurrentSceneDisplay && this._MainMenuClips.Length != 0)
                        this._AudioSource.clip = this._MainMenuClips[2];
                    else if (SceneManager.GetSceneByBuildIndex(2).name == this.CurrentSceneDisplay && this._MainLevelClips.Length != 0)
                        this._AudioSource.clip = this._MainLevelClips[2];
                    break;
                default:
                    break;
            }
            this._AudioSource.Play();
        }
    }
}