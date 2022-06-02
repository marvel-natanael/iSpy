using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject panelMenu, panelSetting, panelChangeName, creditsPanel;
        [SerializeField] private GameObject serverBrowserUI;

        [SerializeField] private Slider bg;
        [SerializeField] private Slider sfx;

        private bool isSBShown = false;

        public RectTransform mainMenu, serverMenu;

        private void Start()
        {
            SetMenu(true, false, false, false);
            B_HideServerBrowser();
        }

        private void Update()
        {
            SetValue();
        }

        private void SetValue()
        {
            PlayerPrefs.SetFloat(Constant.BACKGROUND_AUDIO, bg.value);
            PlayerPrefs.SetFloat(Constant.SFX_AUDIO, sfx.value);
        }

        public void Play()
        {
            SceneManager.LoadScene("Lobby");
        }

        public void Setting()
        {
            SetMenu(false, true, false, false);
        }

        public void Back()
        {
            SetMenu(true, false, false, false);
        }

        public void Credits()
        {
            SetMenu(false, false, false, true);
        }

        private void SetMenu(bool menuActive, bool settingActive, bool changeName, bool credits)
        {
            panelMenu.SetActive(menuActive);
            panelSetting.SetActive(settingActive);
            panelChangeName.SetActive(changeName);
            creditsPanel.SetActive(credits);
        }

        public void ChangeName()
        {
            SetMenu(false, false, true, false);
        }

        public void B_ShowServerBrowser()
        {
            if (isSBShown) return;
            mainMenu.DOAnchorPos(new Vector2(-2000, 0), 1.0f);
            serverMenu.DOAnchorPos(new Vector2(0, 0), 1.0f);
            isSBShown = true;
            serverBrowserUI.SetActive(true);
        }

        public void B_HideServerBrowser()
        {
            if (!isSBShown) return;
            mainMenu.DOAnchorPos(new Vector2(0, 0), 1.0f);
            serverMenu.DOAnchorPos(new Vector2(2000, 0), 1.0f);
            isSBShown = false;
            serverBrowserUI.SetActive(false);
        }
    }
}