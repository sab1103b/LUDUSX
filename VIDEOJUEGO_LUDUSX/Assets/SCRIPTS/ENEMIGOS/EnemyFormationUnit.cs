using UnityEngine;

public class EnemyFormationUnit : MonoBehaviour
{
    public enum EnemyState
    {
        Formation,
        Attacking,
        Returning
    }

    public EnemyState state = EnemyState.Formation;

    public Vector3 formationPosition;
    public Transform player;

    public float speed = 5f;

    private float attackTimer;

    void Update()
    {
        switch (state)
        {
            case EnemyState.Formation:
                FormationMovement();
                break;

            case EnemyState.Attacking:
                AttackMovement();
                break;

            case EnemyState.Returning:
                ReturnToFormation();
                break;
        }
    }

    void FormationMovement()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            formationPosition,
            Time.deltaTime * 2
        );
    }

    void AttackMovement()
    {
        Vector3 dir = (player.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        attackTimer += Time.deltaTime;

        if (attackTimer > 3f)
        {
            state = EnemyState.Returning;
        }
    }

    void ReturnToFormation()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            formationPosition,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, formationPosition) < 0.1f)
        {
            state = EnemyState.Formation;
            attackTimer = 0;
        }
    }

    public void StartAttack()
    {
        state = EnemyState.Attacking;
    }
}