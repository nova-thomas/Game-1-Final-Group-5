using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int coins = 0;
    private bool redKey = false;
    private bool greenKey = false;
    private bool blueKey = false;
    public int firePower = 0;
    public int icePower = 0;
    public int poisonPower = 0;

    public Shop shop;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI firePowerText;
    public TextMeshProUGUI icePowerText;
    public TextMeshProUGUI poisonPowerText;

    private void Start()
    {
        UpdateCoinUI(); 
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        UpdateCoinUI();
        Debug.Log("Picked up " + amount + " coins. Total coins: " + coins);
        if (shop != null && shop.isPlayerNear)
        {
            shop.UpdateCoinDisplay(coins);
        }
    }

    public void AddKey(string keyColor)
    {
        switch (keyColor)
        {
            case "Blue":
                blueKey = true;
                Debug.Log("Blue key collected!");
                break;
            case "Red":
                redKey = true;
                Debug.Log("Red key collected!");
                break;
            case "Green":
                greenKey = true;
                Debug.Log("Green key collected!");
                break;
        }
    }

    public bool HasKey(string keyColor)
    {
        return keyColor switch
        {
            "Blue" => blueKey,
            "Red" => redKey,
            "Green" => greenKey,
            _ => false,
        };
    }

    public void RemoveKey(string keyColor)
    {
        switch (keyColor)
        {
            case "Blue":
                blueKey = false;
                break;
            case "Red":
                redKey = false;
                break;
            case "Green":
                greenKey = false;
                break;
        }
    }

    public void AddFirePower()
    {
        firePower++;
        Debug.Log("Fire power-up collected! Total Fire Power: " + firePower);
        UpdatePowerUpUI();
    }

    public void AddIcePower()
    {
        icePower++;
        Debug.Log("Ice power-up collected! Total Ice Power: " + icePower);
        UpdatePowerUpUI();
    }

    public void AddPoisonPower()
    {
        poisonPower++;
        Debug.Log("Poison power-up collected! Total Poison Power: " + poisonPower);
        UpdatePowerUpUI();
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = coins.ToString(); 
        }
    }

    public void UpdatePowerUpUI()
    {
        if (firePowerText != null)
        {
            firePowerText.text = firePower.ToString();
        }
        if (icePowerText != null)
        {
            icePowerText.text = icePower.ToString();
        }
        if (poisonPowerText != null)
        {
            poisonPowerText.text = poisonPower.ToString();
        }
    }
}
