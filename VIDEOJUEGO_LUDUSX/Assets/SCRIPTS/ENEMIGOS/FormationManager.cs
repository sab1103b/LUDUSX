using UnityEngine;
using System.Collections.Generic;

public class FormationManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int rows = 4;
    public int columns = 5;

    public float spacingX = 2.5f;
    public float spacingY = 1.5f;

    public Transform player;

    private List<EnemyFormationUnit> enemies = new List<EnemyFormationUnit>();


    void Start()
    {
        CreateFormation();
    }


    public float attackInterval = 2f;
    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackInterval)
        {
            SendRandomEnemy();
            attackTimer = 0;
        }
    }

    void SendRandomEnemy()
    {
        if (enemies.Count == 0) return;

        int index = Random.Range(0, enemies.Count);

        if (enemies[index].state == EnemyFormationUnit.EnemyState.Formation)
        {
            enemies[index].StartAttack();
        }
    }

    void CreateFormation()
    {
        Vector3 startPos = transform.position;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 pos = startPos + new Vector3(
                    c * spacingX,
                    -r * spacingY,
                    0
                );

                GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);

                EnemyFormationUnit unit = enemy.AddComponent<EnemyFormationUnit>();

                unit.formationPosition = pos;
                unit.player = player;

                enemies.Add(unit);
            }
        }
    }
}