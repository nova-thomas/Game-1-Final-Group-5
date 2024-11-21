using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public Transform respawnLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathPlane"))
        {
            if (respawnLocation != null)
            {
                transform.position = respawnLocation.position;
            }
        }
    }
}
