using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool AutoSprintEnabled = false;

    public int coins = 0;
    public bool redKey = false;
    public bool greenKey = false;
    public bool blueKey = false;
    public int firePower = 0;
    public int icePower = 0;
    public int poisonPower = 0;

    public float damage = 1;

    public double iceUpgrade = 1;
    public double fireUpgrade = 1;
    public double poisonUpgrade = 1;

    public int ammo = 6;
    public int ammoMax = 6;
    public int health = 20;
    public int healthMax = 20;
    public int armor = 0;
    public int armorMax = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            ResetGameState();
        }
    }

    private void ResetGameState()
    {
        AutoSprintEnabled = false;
        coins = 0;
        redKey = false;
        greenKey = false;
        blueKey = false;
        firePower = 0;
        icePower = 0;
        poisonPower = 0;
        damage = 1;
        iceUpgrade = 1;
        fireUpgrade = 1;
        poisonUpgrade = 1;
        ammo = 6;
        ammoMax = 6;
        health = 20;
        healthMax = 20;
        armor = 0;
        armorMax = 5;
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        Debug.Log("Picked up " + amount + " coins. Total coins: " + coins);
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
    }

    public void AddIcePower()
    {
        icePower++;
        Debug.Log("Ice power-up collected! Total Ice Power: " + icePower);
    }

    public void AddPoisonPower()
    {
        poisonPower++;
        Debug.Log("Poison power-up collected! Total Poison Power: " + poisonPower);
    }
}
