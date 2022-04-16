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

    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    void Start()
    {
        nameInputField = GameObject.Find("Name Input Field").GetComponent<TMP_InputField>();
    }

    public void SetPlayerName()
    {
        if (nameInputField.text != "")
        {
            displayName = nameInputField.text;
            PlayerPrefs.SetString(PlayerPrefsNameKey, displayName);
            //OnClicked();
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            messageError.text = "Username must be filled";
        }
    }
}
