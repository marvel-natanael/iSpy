using System;
using UnityEngine;

namespace MainMenu
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            var audioBackground = GameObject.FindGameObjectsWithTag("AudioManager");

            if (audioBackground.Length > 1)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            var volume = PlayerPrefs.GetFloat(Constant.BACKGROUND_AUDIO, 1f);
            _audioSource.volume = volume;
        }
    }
}
