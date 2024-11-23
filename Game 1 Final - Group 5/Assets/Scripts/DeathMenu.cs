using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public Transform player;
    public Transform respawnPoint;

    public void RespawnPlayer()
    {
        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
            player.transform.rotation = respawnPoint.rotation; // Optional: match rotation of respawn point
            Debug.Log("Player respawned at " + respawnPoint.position);
        }


        gameObject.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
