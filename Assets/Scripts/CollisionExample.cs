using UnityEngine;

public class CollectableSpawner2 : MonoBehaviour
{
    [SerializeField] private GameObject[] collectablePrefabs; 
    [SerializeField] private int minSpawn = 2;
    [SerializeField] private int maxSpawn = 5;
    [SerializeField] private float spawnHeight = 1f; 
    [SerializeField] private float spawnRange = 2f; 

    private void Start()
    {
        SpawnCollectables();
    }

    private void SpawnCollectables()
    {
        int count = Random.Range(minSpawn, maxSpawn + 1);

        for (int i = 0; i < count; i++)
        {
            Vector3 position = transform.position;

            position.y += spawnHeight;
            position.x += Random.Range(-spawnRange, spawnRange);

            //random vegetable
            GameObject prefab = collectablePrefabs[Random.Range(0, collectablePrefabs.Length)];
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}
