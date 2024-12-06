using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public Shop shop;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI firePowerText;
    public TextMeshProUGUI icePowerText;
    public TextMeshProUGUI poisonPowerText;

    private void Start()
    {
        UpdateCoinUI();
        UpdatePowerUpUI();
    }

    public void AddCoin(int amount)
    {
        GameManager.Instance.AddCoin(amount);
        UpdateCoinUI();

        if (shop != null && shop.isPlayerNear)
        {
            shop.UpdateCoinDisplay(GameManager.Instance.coins);
        }
    }

    public void AddKey(string keyColor)
    {
        GameManager.Instance.AddKey(keyColor);
    }

    public bool HasKey(string keyColor)
    {
        return GameManager.Instance.HasKey(keyColor);
    }

    public void RemoveKey(string keyColor)
    {
        GameManager.Instance.RemoveKey(keyColor);
    }

    public void AddFirePower()
    {
        GameManager.Instance.AddFirePower();
        UpdatePowerUpUI();
    }

    public void AddIcePower()
    {
        GameManager.Instance.AddIcePower();
        UpdatePowerUpUI();
    }

    public void AddPoisonPower()
    {
        GameManager.Instance.AddPoisonPower();
        UpdatePowerUpUI();
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = GameManager.Instance.coins.ToString();
        }
    }

    public void UpdatePowerUpUI()
    {
        if (firePowerText != null)
        {
            firePowerText.text = GameManager.Instance.firePower.ToString();
        }
        if (icePowerText != null)
        {
            icePowerText.text = GameManager.Instance.icePower.ToString();
        }
        if (poisonPowerText != null)
        {
            poisonPowerText.text = GameManager.Instance.poisonPower.ToString();
        }
    }
}
