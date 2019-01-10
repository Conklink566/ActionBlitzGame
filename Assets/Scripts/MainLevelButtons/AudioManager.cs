using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Interface
{
    /// <summary>
    /// Type of sound effects
    /// </summary>
    public enum SoundEffectType
    {
        Jump,
        Land,
        Hit,
        Slide
    };

    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// Create this instance
        /// </summary>
        public static AudioManager Instance;

        /// <summary>
        /// Get this component
        /// </summary>
        public AudioSource AudioSource { get; private set; }

        /// <summary>
        /// Jump Sound
        /// </summary>
        [SerializeField]
        private SoundEffectObject _JumpSound;

        /// <summary>
        /// Land Sound
        /// </summary>
        [SerializeField]
        private SoundEffectObject _LandSound;

        /// <summary>
        /// GotHit Sound
        /// </summary>
        [SerializeField]
        private SoundEffectObject _GotHitSound;

        /// <summary>
        /// Slide Sound
        /// </summary>
        [SerializeField]
        private SoundEffectObject _SlideSound;

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
            this.AudioSource = this.GetComponent<AudioSource>();
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
            if (SceneManager.GetActiveScene().name == SceneManager.GetSceneByBuildIndex((int)SceneNames.LoadingLevel).name ||
                SceneManager.GetActiveScene().name == SceneManager.GetSceneByBuildIndex((int)SceneNames.LoadingAssets).name)
                return;
            this.AudioSource.Stop();
            switch(type)
            {
                case EnvironmentTypes.CityDay:
                    if(SceneManager.GetSceneByBuildIndex(0).name == this.CurrentSceneDisplay && this._MainMenuClips.Length != 0)
                        this.AudioSource.clip = this._MainMenuClips[0];
                    else if (SceneManager.GetSceneByBuildIndex(2).name == this.CurrentSceneDisplay && this._MainLevelClips.Length != 0)
                        this.AudioSource.clip = this._MainLevelClips[0];
                    break;
                case EnvironmentTypes.CityNight:
                    if (SceneManager.GetSceneByBuildIndex(0).name == this.CurrentSceneDisplay && this._MainMenuClips.Length != 0)
                        this.AudioSource.clip = this._MainMenuClips[1];
                    else if (SceneManager.GetSceneByBuildIndex(2).name == this.CurrentSceneDisplay && this._MainLevelClips.Length != 0)
                        this.AudioSource.clip = this._MainLevelClips[1];
                    break;
                case EnvironmentTypes.CitySunset:
                    if (SceneManager.GetSceneByBuildIndex(0).name == this.CurrentSceneDisplay && this._MainMenuClips.Length != 0)
                        this.AudioSource.clip = this._MainMenuClips[2];
                    else if (SceneManager.GetSceneByBuildIndex(2).name == this.CurrentSceneDisplay && this._MainLevelClips.Length != 0)
                        this.AudioSource.clip = this._MainLevelClips[2];
                    break;
                default:
                    break;
            }
            this.AudioSource.Play();
        }

        /// <summary>
        /// Create sound effect based off the call
        /// </summary>
        public void CreateSoundEffect(SoundEffectType type, Vector3 position)
        {
            GameObject effect = null;
            switch(type)
            {
                case SoundEffectType.Hit:
                    effect = (GameObject)Instantiate(this._GotHitSound.gameObject);
                    break;
                case SoundEffectType.Jump:
                    effect = (GameObject)Instantiate(this._JumpSound.gameObject);
                    break;
                case SoundEffectType.Land:
                    effect = (GameObject)Instantiate(this._LandSound.gameObject);
                    break;
                case SoundEffectType.Slide:
                    effect = (GameObject)Instantiate(this._SlideSound.gameObject);
                    break;
                default:
                    break;
            }
            effect.transform.position = position;
        }
    }
}