using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomEdgePosition();
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    Vector3 GetRandomEdgePosition()
    {
        int edge = Random.Range(0, 4);
        switch (edge)
        {
            case 0: return new Vector3(-10f, Random.Range(-10f, 10f), 0);
            case 1: return new Vector3(10f, Random.Range(-10f, 10f), 0);
            case 2: return new Vector3(Random.Range(-10f, 10f), -10f, 0);
            default: return new Vector3(Random.Range(-10f, 10f), 10f, 0);
        }
    }
}