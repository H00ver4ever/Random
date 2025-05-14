using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour

{
    public enum EnemyState
    {
        Chase,Patrol
    }

    public Transform player;
    public EnemyState currentState;

    Transform target; 
    NavMeshAgent agent;

    public Transform[] path;
    public int pathIndex = 0;
    public float distThreshold = 0.2f; //floating point math is inexact, this allows us to get close enough to a waypoint and move to the next one. 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            Debug.LogWarning("player null finding with tag");
            player = GameObject.FindWithTag("Player")?.transform;
            
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!player) return;

        if (currentState == EnemyState.Chase) target = player;

        if (currentState == EnemyState.Patrol)
        {
            if (target == player) target = path[pathIndex];

            if (agent.remainingDistance < distThreshold)
            {
                pathIndex++;

                pathIndex %= path.Length;
                target = path[pathIndex];

                //if we reach the end of path - go to zero  
                //if (pathIndex == path.Length) pathIndex = 0; - basically the harder way of doing pathIndex %= path.length

            }
        }
        agent.SetDestination(target.position);
    }
}
