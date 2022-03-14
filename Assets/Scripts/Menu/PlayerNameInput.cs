using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField]
    TMP_InputField nameInputField;
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
        displayName = nameInputField.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, displayName);
        OnClicked();
    }
}
