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

    private bool isPlayerNear = false;
    private int healthUpgradeLevel = 0;
    private int damageUpgradeLevel = 0;
    private int speedUpgradeLevel = 0;
    private const int maxUpgradeLevel = 6;

    private void Start()
    {
        shopUI.SetActive(false);
        healthProgressBar.maxValue = maxUpgradeLevel;
        damageProgressBar.maxValue = maxUpgradeLevel;
        speedProgressBar.maxValue = maxUpgradeLevel;

        healthUpgradeButton.onClick.AddListener(UpgradeHealth);
        damageUpgradeButton.onClick.AddListener(UpgradeDamage);
        speedUpgradeButton.onClick.AddListener(UpgradeSpeed);
        backButton.onClick.AddListener(CloseShop);

        Cursor.lockState = CursorLockMode.Locked;
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
            Debug.Log("Shop opened.");
        }
    }

    private void CloseShop()
    {
        shopUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Shop closed.");
    }

    private void UpgradeHealth()
    {
        if (healthUpgradeLevel < maxUpgradeLevel)
        {
            healthUpgradeLevel++;
            healthProgressBar.value = healthUpgradeLevel;
            Debug.Log("Health upgraded to level " + healthUpgradeLevel);
        }
    }

    private void UpgradeDamage()
    {
        if (damageUpgradeLevel < maxUpgradeLevel)
        {
            damageUpgradeLevel++;
            damageProgressBar.value = damageUpgradeLevel;
            Debug.Log("Damage upgraded to level " + damageUpgradeLevel);
        }
    }

    private void UpgradeSpeed()
    {
        if (speedUpgradeLevel < maxUpgradeLevel)
        {
            speedUpgradeLevel++;
            speedProgressBar.value = speedUpgradeLevel;
            Debug.Log("Speed upgraded to level " + speedUpgradeLevel);
        }
    }

    
}
