using UnityEngine;

public class Pickups : MonoBehaviour
{
    public GameObject GameObject; // Prefab to spawn
    public int spawnCount = 5;

    public Vector3 areaCenter = Vector3.zero;
    public Vector3 areaSize = new Vector3(10, 0, 10);

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(GameObject, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float y = Random.Range(-areaSize.y / 2, areaSize.y / 2);
        float z = Random.Range(-areaSize.z / 2, areaSize.z / 2);

        return areaCenter + new Vector3(x, y, z);
    }
}
