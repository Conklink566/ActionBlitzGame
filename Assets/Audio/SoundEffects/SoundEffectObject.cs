using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interface
{
    public class SoundEffectObject : MonoBehaviour
    {
        /// <summary>
        /// Sound Effect Clip
        /// </summary>
        [SerializeField]
        private AudioClip[] _SoundEffect;

        /// <summary>
        /// Audio Source
        /// </summary>
        private AudioSource _AudioSource;

        /// <summary>
        /// Length of the sound effect
        /// </summary>
        private float _KillTimer;

        /// <summary>
        /// Current Timer
        /// </summary>
        private float _CurrentTimer = 0.0f;

        /// <summary>
        /// Awake this instance
        /// </summary>
        private void Awake()
        {
            this._AudioSource = this.GetComponent<AudioSource>();
        }

        /// <summary>
        /// Start this instance
        /// </summary>
        private void Start()
        {
            this._AudioSource.clip = this._SoundEffect[Random.Range(0, this._SoundEffect.Length - 1)];
            this._KillTimer = this._AudioSource.clip.length;
            this._AudioSource.Play();
        }

        /// <summary>
        /// Update every frame
        /// </summary>
        private void Update()
        {
            this._CurrentTimer += Time.deltaTime;
            if (this._CurrentTimer > this._KillTimer)
            {
                Destroy(this.gameObject);
            }
        }

    }
}