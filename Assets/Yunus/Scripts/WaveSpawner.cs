using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform pathParent;
    public EnemyFollowPath enemyPrefab;

    [Header("Wave")]
    public float timeBetweenWaves = 4f;
    public int enemiesPerWave = 8;
    public float spawnInterval = 0.6f;

    int wave = 0;
    bool spawning;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            wave++;
            int count = enemiesPerWave + wave * 2; // grows each wave
            yield return SpawnWave(count);
        }
    }

    IEnumerator SpawnWave(int count)
    {
        if (spawning) yield break;
        spawning = true;

        for (int i = 0; i < count; i++)
        {
            var e = Instantiate(enemyPrefab);
            e.pathParent = pathParent;
            yield return new WaitForSeconds(spawnInterval);
        }

        spawning = false;
    }
}

