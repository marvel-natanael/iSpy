using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Player.Weapons;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] List<Sprite> weaponSpriteList = new List<Sprite>();
    private Sprite sprite;

    [SerializeField] TMP_Text amountText;

    private WeaponSwap targetPlayer;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetTargetPlayer(WeaponSwap player)
    {
        targetPlayer = player;
    }

    public void UpdateSprite(string type, int amount)
    {
        sprite = GetComponent<Image>().sprite;
        
        gameObject.SetActive(true);

        if (type == "Pistol") 
        {
            sprite = weaponSpriteList[0];
            Debug.Log("SPRITE PISTOL");
        }
        else if(type == "Shotgun")
        {
            sprite = weaponSpriteList[1];
            Debug.Log("SPRITE SHOTGUN");
        }
        else
        {
            Debug.Log("Sprite can't relate");
        }

        amountText.text = amount.ToString();
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}
