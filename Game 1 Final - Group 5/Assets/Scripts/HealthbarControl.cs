using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarControl : MonoBehaviour
{
    public Image healthBarFill;
    private EnemyAi enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyAi>();
        UpdateHealthBar();
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }


    public void UpdateHealthBar()
    {
        // Update the fill amount of the health bar
        float fill = enemy.GetCurrentHealth() / enemy.GetMaxHealth();
        healthBarFill.fillAmount = fill;
    }
}
