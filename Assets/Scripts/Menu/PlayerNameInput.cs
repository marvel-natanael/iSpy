using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInputField;

    [SerializeField] private TMP_Text messageError;
    private const string PlayerPrefsNameKey = "PlayerName";
    public static string DisplayName { get; private set; }

    private void Start()
    {
        nameInputField = GameObject.Find("Name Input Field").GetComponent<TMP_InputField>();
        nameInputField.text = PlayerPrefs.GetString(PlayerPrefsNameKey, "");
    }

    public void SetPlayerName()
    {
        if (nameInputField.text == "") { messageError.text = "Username must be filled"; return; }
        DisplayName = nameInputField.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
        //OnClicked();

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            SceneManager.LoadScene("MainMenu");
            Debug.Log(DisplayName);
        }
    }
}