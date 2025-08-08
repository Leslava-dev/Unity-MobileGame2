using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    public GameObject[] collectables;
    public int amountToSpawn = 10;
    public Vector2 spawnAreaMin = new Vector2(-5, -5);
    public Vector2 spawnAreaMax = new Vector2(5, 5);

    void Start()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                1f,
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            int index = Random.Range(0, collectables.Length);
            Instantiate(collectables[index], spawnPos, Quaternion.identity);
        }
    }
}
