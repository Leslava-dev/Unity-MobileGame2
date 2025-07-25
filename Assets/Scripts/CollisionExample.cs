using UnityEngine;

public class CollisionExample : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("---> Стикнувся з: " + collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("---> Відійшов від: " + collision.gameObject.name);
    }
}
