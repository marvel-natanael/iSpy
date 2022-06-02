using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "Lobby")
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }


        private void Update()
        {
            if (GameObject.FindWithTag("AudioManager") && SceneManager.GetActiveScene().name == "Map")
            {
                Destroy(gameObject);
            }
            
            var volume = PlayerPrefs.GetFloat(Constant.BACKGROUND_AUDIO, 1f);
            _audioSource.volume = volume;
        }
    }
}
