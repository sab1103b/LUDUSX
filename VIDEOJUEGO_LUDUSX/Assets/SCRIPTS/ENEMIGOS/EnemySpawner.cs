using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool pool;
    public Transform player;

    public float spawnDistance = 25f;

    public void SpawnEnemy()
    {
        GameObject enemy = pool.GetEnemy();

        if (enemy == null) return;

        Vector3 spawnPos = new Vector3(
            Random.Range(-8f, 8f),
            Random.Range(2f, 5f),
            player.position.z + spawnDistance
        );

        enemy.transform.position = spawnPos;

        PATRONES pat = enemy.GetComponent<PATRONES>();

        pat.player = player;
        pat.pattern = (PATRONES.MovementPattern)Random.Range(0, 5);
    }
}