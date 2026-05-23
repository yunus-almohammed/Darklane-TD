using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerFixedRounds : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject enemyPrefab;   // e.g. Circle (red), Circle1 (blue)...
        public int count = 5;
        
        public float startTime = 0f;
        public float delayBetweenSpawns = 0.5f; // seconds between each enemy of this group
    }

    [System.Serializable]
    public class Round
    {
        public string name = "Round";
        public float startDelay = 1.5f;              // wait before this round starts
        public List<EnemyGroup> groups = new();       // what spawns in this round
        public float delayAfterRound = 3f;            // rest time after clearing round
    }

    [Header("Rounds (Fixed)")]
    public List<Round> rounds = new();

    [Header("Where to spawn")]
    public Transform spawnPoint;

    [Header("Path to follow")]
    public Transform pathParent;

    [Header("Auto start?")]
    public bool autoStart = true;

    int roundIndex = -1;
    bool running = false;

    void Start()
    {
        if (autoStart) StartNextRound();
    }

    public void StartNextRound()
    {
        if (running) return;

        roundIndex++;
        if (roundIndex >= rounds.Count)
        {
            Debug.Log("ALL ROUNDS COMPLETED!");
            return;
        }

        StartCoroutine(RunRound(rounds[roundIndex]));
    }

    IEnumerator RunRound(Round round)
    {
        running = true;

        if (round.startDelay > 0f)
            yield return new WaitForSeconds(round.startDelay);

        int activeGroups = 0;

        // Spawn all groups in this round
        foreach (var g in round.groups)
        {
            activeGroups++;
            StartCoroutine(SpawnGroup(g, () => activeGroups--));
        }

        yield return new WaitUntil(() => activeGroups == 0);

        // Wait until all enemies are dead
        yield return new WaitUntil(() => EnemyHealth.AliveCount == 0);

        if (round.delayAfterRound > 0f)
            yield return new WaitForSeconds(round.delayAfterRound);

        running = false;

        // Automatically continue to next round (optional)
        StartNextRound();
    }
    IEnumerator SpawnGroup(EnemyGroup g, System.Action onDone)
    {
        if (g.startTime > 0f)
            yield return new WaitForSeconds(g.startTime);

        for (int i = 0; i < g.count; i++)
        {
            var go = Instantiate(g.enemyPrefab, spawnPoint.position, Quaternion.identity);

            var mover = go.GetComponent<EnemyFollowPath>();
            if (mover != null) mover.pathParent = pathParent;

            if (g.delayBetweenSpawns > 0f)
                yield return new WaitForSeconds(g.delayBetweenSpawns);
        }

        onDone?.Invoke();
    }
}

