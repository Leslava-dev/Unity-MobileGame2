using UnityEngine;

public class SimpleCollectableSpawner : MonoBehaviour
{
    public GameObject[] collectables; 
    public Transform[] spawnPoints;   

    public Transform player;
    public float spawnZ = 30f;
    public float interval = 5f;
    public int objectsPerSpawn = 4;

    private float nextSpawnZ = 0f;

    void Update()
    {
        if (player.position.z + spawnZ > nextSpawnZ)
        {
            SpawnObjects();
            nextSpawnZ += interval;
        }
    }

    void SpawnObjects()
{
    if (collectables == null || collectables.Length == 0) return;
    if (spawnPoints == null || spawnPoints.Length == 0) return;

    int count = Mathf.Min(objectsPerSpawn, spawnPoints.Length);

    for (int i = 0; i < count; i++)
        {
            var prefab = collectables[Random.Range(0, collectables.Length)];
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
