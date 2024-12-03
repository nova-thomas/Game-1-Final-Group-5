using System.Collections;
using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
 
    public GameObject coinPrefab; 
    public GameObject powerUpPrefab;
    public GameObject keyPrefab;

    public Transform spawnPoint; 


    public Transform chestTop; 
    public float animationDuration = 0.75f; 
    public float openAngle = -75f; 

    public AudioClip chestOpenSound; 
    public AudioSource audioSource;  

    public GameObject chestCanvas; 

    private bool playerInRange = false; 
    private bool chestOpened = false;  

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && !chestOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        Vector3 pos = spawnPoint.position;
        chestOpened = true; 

        if (audioSource != null && chestOpenSound != null)
        {
            audioSource.PlayOneShot(chestOpenSound);
        }

        StartCoroutine(OpenChestAnimation());

        if (chestCanvas != null)
        {
            chestCanvas.SetActive(false);
        }


        if (coinPrefab != null)
        {
            pos += new Vector3(Random.Range(-0.3f, 0.3f), 0.5f, Random.Range(-0.3f, 0.3f));
            Instantiate(coinPrefab, pos, Quaternion.identity);
        }
         if (powerUpPrefab != null)
        {
            pos += new Vector3(Random.Range(-0.3f, 0.3f), 0.5f, Random.Range(-0.3f, 0.3f));
            Instantiate(powerUpPrefab, pos, Quaternion.identity);
        }
        if (keyPrefab != null)
        {
            pos += new Vector3(Random.Range(-0.3f, 0.3f), 0.5f, Random.Range(-0.3f, 0.3f));
            Instantiate(keyPrefab, pos, Quaternion.identity);
        }

    }

    private IEnumerator OpenChestAnimation()
    {
        Quaternion initialRotation = chestTop.localRotation;
        Quaternion targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x + openAngle,
                                                     initialRotation.eulerAngles.y,
                                                     initialRotation.eulerAngles.z);

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            chestTop.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chestTop.localRotation = targetRotation;

    }
}
