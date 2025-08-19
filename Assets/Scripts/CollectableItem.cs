using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private const string PlayerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            Destroy(gameObject);
        }
    }
}
