using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorIndicator; 
    private bool isPlayerInRange = false;

    void Start()
    {
        if (doorIndicator != null)
        {
            doorIndicator.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange && doorIndicator != null && doorIndicator.activeSelf)
        {
            BillboardIndicator();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (doorIndicator != null && !doorIndicator.activeSelf)
            {
                doorIndicator.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (doorIndicator != null && doorIndicator.activeSelf)
            {
                doorIndicator.SetActive(false);
            }
        }
    }

    private void BillboardIndicator()
    {
        doorIndicator.transform.LookAt(Camera.main.transform);
        doorIndicator.transform.Rotate(0, 180, 0); 
    }
}
