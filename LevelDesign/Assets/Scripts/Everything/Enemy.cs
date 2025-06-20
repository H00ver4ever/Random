using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour


{
    public int enemyID;
    public int health;

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

        public void SaveGamePrepare()
    {
        // Create enemy data for this enemy
        LoadSavemanager.LevelStateData.DataEnemy data =
            new LoadSavemanager.LevelStateData.DataEnemy();

        // Fill in data for current enemy
        data.enemyID = GetInstanceID();
        data.health = health;

        data.posRotScale.posX = transform.position.x;
        data.posRotScale.posY = transform.position.y;
        data.posRotScale.posZ = transform.position.z;

        data.posRotScale.rotX = transform.localEulerAngles.x;
        data.posRotScale.rotY = transform.localEulerAngles.y;
        data.posRotScale.rotZ = transform.localEulerAngles.z;

        data.posRotScale.scaleX = transform.localScale.x;
        data.posRotScale.scaleY = transform.localScale.y;
        data.posRotScale.scaleZ = transform.localScale.z;

        //Add enemy to Game State
        GameManager.StateManager.levelState.enemies.Add(data);
    }

    // Function called when loading is complete
    public void LoadGameComplete()
    {
        // Cycle through enemies and find matching ID
        List<LoadSavemanager.LevelStateData.DataEnemy> enemies =
            GameManager.StateManager.levelState.enemies;

        // Reference to this enemy

        LoadSavemanager.LevelStateData.DataEnemy data = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].enemyID == enemyID)
            {
                // Found enemy. Now break break from loop
                data = enemies[i];
                break;
            }
        }

        // If here and no enemy is found, then it was destroyed when saved. So destroy.
        if (data == null)
        {
            Destroy(gameObject);
            return;
        }

        // Else load enemy data
        enemyID = data.enemyID;
        health = data.health;

        // Set position
        transform.position = new Vector3(data.posRotScale.posX,
            data.posRotScale.posY, data.posRotScale.posZ);

        // Set rotation
        transform.localRotation = Quaternion.Euler(data.posRotScale.rotX,
            data.posRotScale.rotY, data.posRotScale.rotZ);

        // Set scale
        transform.localScale = new Vector3(data.posRotScale.scaleX,
            data.posRotScale.scaleY, data.posRotScale.scaleZ);


        enemies.Remove(data);
    }
}

