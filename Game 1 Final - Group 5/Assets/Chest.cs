using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject coins;
    public GameObject powerup;
    public GameObject key;
    public GameObject chestTop;
    public AudioClip chestOpenSound; // Sound for opening the chest
    private AudioSource audioSource;

    private bool isPlayerInRange = false;
    private bool isChestOpened = false;

    void Start()
    {
        if (chestTop == null)
        {
            chestTop = transform.Find("ChestV2_Top").gameObject;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isPlayerInRange && !isChestOpened && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(OpenChest());
        }
    }

    private IEnumerator OpenChest()
    {
        if (chestOpenSound != null)
        {
            audioSource.PlayOneShot(chestOpenSound);
        }

        isChestOpened = true;

        Quaternion startRotation = chestTop.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(-70, 0, 0);
        float duration = 0.6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            chestTop.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        chestTop.transform.rotation = endRotation;

        StartCoroutine(DropItems());
    }

    private IEnumerator DropItems()
    {
        // Drop the coin
        if (coins != null)
        {
            SpawnItem(coins, transform.position + transform.forward * 0.5f + Vector3.up * 0.5f, transform.forward + Vector3.up);
        }

        // Drop the power-up
        if (powerup != null)
        {
            SpawnItem(powerup, transform.position + transform.forward * 0.5f + Vector3.up * 0.5f, transform.forward + Vector3.up);
        }

        // Drop the key
        if (key != null)
        {
            SpawnItem(key, transform.position + transform.forward * 0.5f + Vector3.up * 0.5f, transform.forward + Vector3.up);
        }

        yield return new WaitForSeconds(0.5f);
    }

    private void SpawnItem(GameObject item, Vector3 spawnPosition, Vector3 direction)
    {
        GameObject spawnedItem = Instantiate(item, spawnPosition, Quaternion.identity);
        Rigidbody rb = spawnedItem.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(direction * 5f, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
