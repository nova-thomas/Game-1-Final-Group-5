using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public Player playerScript;  
    public GameObject deathScreenUI; 
    public Transform respawnPoint;   

    private bool isDead = false;

    void Update()
    {
        if (isDead)
        {
            playerScript.DisablePlayerControls();
        }
    }

    public void ShowDeathMenu()
    {
        isDead = true;
        deathScreenUI.SetActive(true);
        playerScript.UnlockCursor();
        playerScript.DisablePlayerControls();
        Time.timeScale = 0f; 
    }

    public void Respawn()
    {
        isDead = false;
        playerScript.Respawn(respawnPoint.position);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}
