using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopUI;
    public Inventory player;

    public Button healthUpgradeButton;
    public Button damageUpgradeButton;
    public Button speedUpgradeButton;
    public Button damageButton;
    public Button iceButton;
    public Button fireButton;
    public Button poisonButton;
    public Button backButton;

    public Slider healthProgressBar;
    public Slider damageProgressBar;
    public Slider speedProgressBar;
    public Slider mainDamageProgressBar;
    public Slider iceDamageProgressBar;
    public Slider fireDamageProgressBar;
    public Slider poisonDamageProgressBar;

    public TextMeshProUGUI coinCountText;
    public TextMeshProUGUI healthCostText;
    public TextMeshProUGUI damageCostText;
    public TextMeshProUGUI speedCostText;

    public AudioSource audioSource;
    public AudioClip upgradeSound;
    public AudioClip buttonSound;

    public bool isPlayerNear = false;

    private int healthUpgradeLevel = 0;
    private int damageUpgradeLevel = 0;
    private int speedUpgradeLevel = 0;

    private int mainDamageUpgradeLevel = 0;
    private int iceDamageUpgradeLevel = 0;
    private int fireDamageUpgradeLevel = 0;
    private int poisonDamageUpgradeLevel = 0;

    private const int maxUpgradeLevel = 5;
    private const int upgradeCost = 5;

    private void Start()
    {
        shopUI.SetActive(false);

        // Initialize sliders
        healthProgressBar.maxValue = maxUpgradeLevel;
        damageProgressBar.maxValue = maxUpgradeLevel;
        speedProgressBar.maxValue = maxUpgradeLevel;
        mainDamageProgressBar.maxValue = maxUpgradeLevel;
        iceDamageProgressBar.maxValue = maxUpgradeLevel;
        fireDamageProgressBar.maxValue = maxUpgradeLevel;
        poisonDamageProgressBar.maxValue = maxUpgradeLevel;

        healthCostText.text = "5 Coins";
        damageCostText.text = "5 Coins";
        speedCostText.text = "5 Coins";

        // Remove old listeners
        healthUpgradeButton.onClick.RemoveAllListeners();
        damageUpgradeButton.onClick.RemoveAllListeners();
        speedUpgradeButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        // Add new listeners
        healthUpgradeButton.onClick.AddListener(UpgradeHealth);
        damageUpgradeButton.onClick.AddListener(UpgradeAmmo);
        speedUpgradeButton.onClick.AddListener(UpgradeSpeed);
        damageButton.onClick.AddListener(UpgradeDamage);
        iceButton.onClick.AddListener(UpgradeIceDamage);
        fireButton.onClick.AddListener(UpgradeFireDamage);
        poisonButton.onClick.AddListener(UpgradePoisonDamage);
        backButton.onClick.AddListener(CloseShop);

        Cursor.lockState = CursorLockMode.Locked;

        UpdateCoinDisplay(GameManager.Instance.coins);
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
            Time.timeScale = 0f;
            shopUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UpdateCoinDisplay(GameManager.Instance.coins);
            Debug.Log("Shop opened.");
        }
    }

    public void CloseShop()
    {
        Time.timeScale = 1f;
        shopUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Shop closed.");
    }

    private void PlaySound(bool isUpgradeAllowed)
    {
        audioSource.clip = isUpgradeAllowed ? upgradeSound : buttonSound;
        audioSource.Play();
    }

    private void UpgradeHealth()
    {
        bool canUpgrade = healthUpgradeLevel < maxUpgradeLevel && GameManager.Instance.coins >= upgradeCost;

        PlaySound(canUpgrade);

        if (canUpgrade)
        {
            GameManager.Instance.coins -= upgradeCost;
            GameManager.Instance.healthMax += 3;
            healthUpgradeLevel++;
            healthProgressBar.value = healthUpgradeLevel;
            GameManager.Instance.health = GameManager.Instance.healthMax;
            UpdateCoinCount();
            player.UpdateCoinUI();
            Debug.Log("Health upgraded to level " + healthUpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough coins or max level reached.");
        }
    }

    private void UpgradeAmmo()
    {
        bool canUpgrade = damageUpgradeLevel < maxUpgradeLevel && GameManager.Instance.coins >= upgradeCost;

        PlaySound(canUpgrade);

        if (canUpgrade)
        {
            GameManager.Instance.coins -= upgradeCost;
            GameManager.Instance.ammoMax += 3;
            damageUpgradeLevel++;
            damageProgressBar.value = damageUpgradeLevel;
            UpdateCoinCount();
            player.UpdateCoinUI();
            Debug.Log("Ammo upgraded to level " + damageUpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough coins or max level reached.");
        }
    }

    private void UpgradeSpeed()
    {
        bool canUpgrade = speedUpgradeLevel < maxUpgradeLevel && GameManager.Instance.coins >= upgradeCost;

        PlaySound(canUpgrade);

        if (canUpgrade)
        {
            GameManager.Instance.coins -= upgradeCost;
            GameManager.Instance.armor += 2;
            speedUpgradeLevel++;
            speedProgressBar.value = speedUpgradeLevel;
            UpdateCoinCount();
            player.UpdateCoinUI();
            Debug.Log("Speed upgraded to level " + speedUpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough coins or max level reached.");
        }
    }

    private void UpgradeDamage()
    {
        bool canUpgrade = mainDamageUpgradeLevel < maxUpgradeLevel && GameManager.Instance.coins >= upgradeCost;

        PlaySound(canUpgrade);

        if (canUpgrade)
        {
            GameManager.Instance.coins -= upgradeCost;
            GameManager.Instance.damage += 0.5f;
            mainDamageUpgradeLevel++;
            mainDamageProgressBar.value = mainDamageUpgradeLevel;
            UpdateCoinCount();
            player.UpdateCoinUI();
            Debug.Log("Damage upgraded to level " + mainDamageUpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough coins or max level reached.");
        }
    }

    private void UpgradeIceDamage()
    {
        bool canUpgrade = iceDamageUpgradeLevel < maxUpgradeLevel && GameManager.Instance.coins >= upgradeCost;

        PlaySound(canUpgrade);

        if (canUpgrade)
        {
            GameManager.Instance.coins -= upgradeCost;
            GameManager.Instance.iceUpgrade += 0.2f;
            iceDamageUpgradeLevel++;
            iceDamageProgressBar.value = iceDamageUpgradeLevel;
            UpdateCoinCount();
            player.UpdateCoinUI();
            Debug.Log("Ice damage upgraded to level " + iceDamageUpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough coins or max level reached.");
        }
    }

    private void UpgradeFireDamage()
    {
        bool canUpgrade = fireDamageUpgradeLevel < maxUpgradeLevel && GameManager.Instance.coins >= upgradeCost;

        PlaySound(canUpgrade);

        if (canUpgrade)
        {
            GameManager.Instance.coins -= upgradeCost;
            GameManager.Instance.fireUpgrade += 0.2f;
            fireDamageUpgradeLevel++;
            fireDamageProgressBar.value = fireDamageUpgradeLevel;
            UpdateCoinCount();
            player.UpdateCoinUI();
            Debug.Log("Fire damage upgraded to level " + fireDamageUpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough coins or max level reached.");
        }
    }

    private void UpgradePoisonDamage()
    {
        bool canUpgrade = poisonDamageUpgradeLevel < maxUpgradeLevel && GameManager.Instance.coins >= upgradeCost;

        PlaySound(canUpgrade);

        if (canUpgrade)
        {
            GameManager.Instance.coins -= upgradeCost;
            GameManager.Instance.poisonUpgrade += 0.2f;
            poisonDamageUpgradeLevel++;
            poisonDamageProgressBar.value = poisonDamageUpgradeLevel;
            UpdateCoinCount(); 
            player.UpdateCoinUI();
            Debug.Log("Poison damage upgraded to level " + poisonDamageUpgradeLevel);
        }
        else
        {
            Debug.Log("Not enough coins or max level reached.");
        }
    }

    private void UpdateCoinCount()
    {
        coinCountText.text = GameManager.Instance.coins.ToString();
    }

    public void UpdateCoinDisplay(int coins)
    {
        coinCountText.text = coins.ToString();
    }
}
