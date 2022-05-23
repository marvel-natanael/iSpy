using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField]
    TMP_InputField nameInputField;

    [SerializeField] private TMP_Text messageError;
    private const string PlayerPrefsNameKey = "PlayerName";
    public static string displayName { get; private set; }

    void Start()
    {
        nameInputField = GameObject.Find("Name Input Field").GetComponent<TMP_InputField>();
        nameInputField.text = PlayerPrefs.GetString(PlayerPrefsNameKey, "");
    }

    public void SetPlayerName()
    {
        if (nameInputField.text != "")
        {
            displayName = nameInputField.text;
            PlayerPrefs.SetString(PlayerPrefsNameKey, displayName);
            //OnClicked();

            if (SceneManager.GetActiveScene().name == "Menu")
            {
                SceneManager.LoadScene("MainMenu");
                Debug.Log(displayName);
            }
        }
        else
        {
            messageError.text = "Username must be filled";
        }
    }
}
