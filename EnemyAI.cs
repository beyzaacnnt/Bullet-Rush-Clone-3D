using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum State { Idle, Chase, Attack }
    public State currentState;

    public Transform target;
    public float chaseRange = 15f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Idle;
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case State.Idle:
                if (distanceToTarget <= chaseRange)
                    currentState = State.Chase;
                break;

            case State.Chase:
                agent.SetDestination(target.position);
                if (distanceToTarget <= attackRange)
                    currentState = State.Attack;
                else if (distanceToTarget > chaseRange)
                    currentState = State.Idle;
                break;

            case State.Attack:
                agent.SetDestination(transform.position); // Dur ve saldır
                transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
                
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    PerformAttack();
                }

                if (distanceToTarget > attackRange)
                    currentState = State.Chase;
                break;
        }
    }

    void PerformAttack()
    {
        // Saldırı animasyonu veya hasar hesaplama kodları
        lastAttackTime = Time.time;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
