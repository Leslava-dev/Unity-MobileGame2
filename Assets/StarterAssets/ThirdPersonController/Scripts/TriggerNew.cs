using UnityEngine;

public class TriggerNew : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("---> Увійшов об'єкт: " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("---> Вийшов об'єкт: " + other.name);
    }
}
