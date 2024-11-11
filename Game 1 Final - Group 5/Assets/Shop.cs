using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Player player;
    public GameObject shopUI;
    public Button healthUpgradeButton;
    public Button damageUpgradeButton;
    public Button speedUpgradeButton;
    public Button backButton;
    public Slider healthProgressBar;
    public Slider damageProgressBar;
    public Slider speedProgressBar;
    public TextMeshProUGUI coinCountText;
    public TextMeshProUGUI healthCostText;
    public TextMeshProUGUI damageCostText;
    public TextMeshProUGUI speedCostText;

    public bool isPlayerNear = false;
    private int healthUpgradeLevel = 0;
    private int damageUpgradeLevel = 0;
    private int speedUpgradeLevel = 0;
    private const int maxUpgradeLevel = 12;
    private const int upgradeCost = 5;
    private void Start()
    {
       
            shopUI.SetActive(false);
            healthProgressBar.maxValue = maxUpgradeLevel;
            damageProgressBar.maxValue = maxUpgradeLevel;
            speedProgressBar.maxValue = maxUpgradeLevel;

            healthCostText.text = "10 Coins";
            damageCostText.text = "10 Coins";
            speedCostText.text = "10 Coins";

            healthUpgradeButton.onClick.RemoveAllListeners();
            damageUpgradeButton.onClick.RemoveAllListeners();
            speedUpgradeButton.onClick.RemoveAllListeners();
            backButton.onClick.RemoveAllListeners();

            healthUpgradeButton.onClick.AddListener(UpgradeHealth);
            damageUpgradeButton.onClick.AddListener(UpgradeDamage);
            speedUpgradeButton.onClick.AddListener(UpgradeSpeed);
            backButton.onClick.AddListener(CloseShop);

            Cursor.lockState = CursorLockMode.Locked;
        
        UpdateCoinDisplay(player.inventory.coins);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Player near the shop.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player left the shop.");
        }
    }

    public void Interact()
    {
        if (isPlayerNear)
        {
            shopUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UpdateCoinDisplay(player.inventory.coins); 
            Debug.Log("Shop opened.");
        }
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Shop closed.");
    }

    private void UpgradeHealth()
    {
        if (healthUpgradeLevel < maxUpgradeLevel && player.inventory.coins >= 5)
        {
            player.inventory.coins -= 5;  
            player.healthMax+= 3;  
            healthUpgradeLevel++;
            healthProgressBar.value = healthUpgradeLevel;
            coinCountText.text = "Coins: " + player.inventory.coins;
            Debug.Log("Health upgraded to level " + healthUpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough coins or max level reached.");
        }
    }

    private void UpgradeDamage()
    {
        if (damageUpgradeLevel < maxUpgradeLevel && player.inventory.coins >= 5)
        {
            player.inventory.coins -= 5; 
            player.ammoMax += 3; 
            damageUpgradeLevel++;
            damageProgressBar.value = damageUpgradeLevel;
            coinCountText.text = "Coins: " + player.inventory.coins;
            Debug.Log("Damage upgraded to level " + damageUpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough coins or max level reached.");
        }
    }

    private void UpgradeSpeed()
    {
        if (speedUpgradeLevel < maxUpgradeLevel && player.inventory.coins >= 5)
        {
            player.inventory.coins -= 5; 
            player.speed += 2;  
            speedUpgradeLevel++;
            speedProgressBar.value = speedUpgradeLevel;
            coinCountText.text = "Coins: " + player.inventory.coins;
            Debug.Log("Speed upgraded to level " + speedUpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough coins or max level reached.");
        }
    }

    private void UpdateCoinCount()
    {
        coinCountText.text = "Coins: " + player.inventory.coins;
    }

    public void UpdateCoinDisplay(int coins)
    {
        coinCountText.text = "" + coins;
    }


}
