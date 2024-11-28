using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider; 
     public GameObject bgmPlayer;

    public Toggle autoSprintToggle;
    private DDOL bgmScript;

    void Start()
    {
        if (bgmPlayer != null)
        {
            bgmScript = bgmPlayer.GetComponent<DDOL>();

            if (bgmScript != null)
            {
                volumeSlider.value = bgmScript.GetVolume();

                volumeSlider.onValueChanged.AddListener(bgmScript.SetVolume);
            }
        }

        autoSprintToggle.isOn = GameManager.Instance.AutoSprintEnabled;

        autoSprintToggle.onValueChanged.AddListener(OnAutoSprintToggleChanged);
    }

    private void OnAutoSprintToggleChanged(bool isOn)
    {
        GameManager.Instance.AutoSprintEnabled = isOn;

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.ToggleSprintingState(isOn);
        }
    }
}
