using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int monsterCount = 3;

    [Header("Spawn Area")]
    public Vector2 spawnAreaMin = new Vector2(-10, -10);
    public Vector2 spawnAreaMax = new Vector2(10, 10);

    [Header("Spawn Distance From Player")]
    public float minDistanceFromPlayer = 5f;

    public Item[] possibleLoot;

    private Transform player;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            player = playerGO.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure Player has 'Player' tag.");
            return;
        }

        for (int i = 0; i < monsterCount; i++)
        {
            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        GameObject monsterGO = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

        // Assign random loot
        MonsterHealth monsterHealth = monsterGO.GetComponent<MonsterHealth>();
        if (monsterHealth != null && possibleLoot.Length > 0)
        {
            monsterHealth.lootDropItem = possibleLoot[Random.Range(0, possibleLoot.Length)];
        }

        spawnedMonsters.Add(monsterGO);
        Debug.Log("Monster spawned at: " + spawnPosition);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPos = Vector3.zero;
        bool validPosition = false;
        int maxAttempts = 50;  // Prevent infinite loops
        int attempts = 0;

        while (!validPosition && attempts < maxAttempts)
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            spawnPos = new Vector3(x, y, 0);

            // Check distance from player
            if (Vector3.Distance(spawnPos, player.position) >= minDistanceFromPlayer)
            {
                validPosition = true;
            }

            attempts++;
        }

        if (!validPosition)
        {
            Debug.LogWarning("Could not find valid spawn position after " + maxAttempts + " attempts. Spawning at current position.");
        }

        return spawnPos;
    }

    public List<GameObject> GetSpawnedMonsters()
    {
        return spawnedMonsters;
    }
}

