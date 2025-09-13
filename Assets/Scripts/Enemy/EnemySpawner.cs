using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawn;
    [SerializeField] private float bossSpawnTime = 15f;
    [SerializeField] private float timeBetweenSpawn = 2f;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnBoss());
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitUntil(() => GameManager.Instance.state == GameState.InProgress);

        if (GameManager.Instance.state == GameState.InProgress)
        {
            yield return new WaitForSeconds(bossSpawnTime);
            Instantiate(bossPrefab, bossSpawn.position, Quaternion.identity);
        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitUntil(() => GameManager.Instance.state == GameState.InProgress);

        while (GameManager.Instance.state == GameState.InProgress)
        {
            GameObject enemies = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var enemy = Instantiate(enemies, spawnPoint.position, Quaternion.identity);
            SpriteRenderer enemySr = enemy.GetComponent<SpriteRenderer>();
            enemySr.sortingLayerName = "Layer2";
            enemies.layer = gameObject.layer;
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }
}
