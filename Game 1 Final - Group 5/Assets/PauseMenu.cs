using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseScreenUI; 
    public Player playerScript;    
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menu"); 
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
