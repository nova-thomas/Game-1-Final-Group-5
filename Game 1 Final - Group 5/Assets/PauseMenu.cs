using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseScreenUI; // Assign in the inspector
    public Player playerScript;     // Reference to the Player script
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Example: Toggle with the Escape key
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseScreenUI.SetActive(true);
        Time.timeScale = 0f;
        UnlockCursor();
        playerScript.canShoot = false;
        playerScript.canJump = false;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseScreenUI.SetActive(false);
        Time.timeScale = 1f;
        LockCursor();
        playerScript.canShoot = true;
        playerScript.canJump = true;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene("Menu"); // Replace "MainMenu" with your scene name
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
