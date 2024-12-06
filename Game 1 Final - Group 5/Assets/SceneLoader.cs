using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            if (sceneName == "Menu")
            {
                SceneManager.LoadScene(sceneName);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 1f;
                return;
            }
            SceneManager.LoadScene(sceneName);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }
}
