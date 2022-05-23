using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryObject : MonoBehaviour
{
    public readonly int id;
    private TextMeshProUGUI roomNumber;
    private TextMeshProUGUI playerCount;
    private TextMeshProUGUI status;
    private TextMeshProUGUI portAddress;

    public string RoomNumber => roomNumber.text;
    public string PortAddress => portAddress.text;
    public string PlayerCount => playerCount.text;
    public string Status => status.text;

    public void Awake()
    {
        if (transform.childCount != 4) return;
        roomNumber = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        playerCount = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        status = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        portAddress = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }

    public void UpdateData(int _roomNumber, ServerDataEntry entry)
    {
        roomNumber.text = _roomNumber.ToString();
        portAddress.text = entry.Port.ToString();
        playerCount.text = $"Players: {entry.PlayerCount} / {entry.MaxPlayer}";
        if (entry.Running | entry.PlayerCount >= entry.MaxPlayer)
        {
            GetComponent<Button>().interactable = false;
            status.text = $"Status:\nRunning";
        }
        else
        {
            GetComponent<Button>().interactable = true;
            status.text = $"Status:\nWaiting";
        }
    }

    public void SetSelected()
    {
        ServerBrowserScript.Singleton.CurrentSelected = this;
    }
}