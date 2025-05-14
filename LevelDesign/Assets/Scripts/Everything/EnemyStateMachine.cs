using UnityEngine;
using StateMachine;
using StateMachine.Editor;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine<EnemyContext>
{
    public IState<EnemyContext> idleState;
    public IState<EnemyContext> patrolState;
    public IState<EnemyContext> chaseState;

    
    void Start()
    {
        context = new EnemyContext()
        {
            baseHealth = 50,
            CurrentHealth = 50,
            maxHealth = 100,
            agent = GetComponent<NavMeshAgent>()
        };

        StateBuilder<EnemyContext> stateBuilder = new StateBuilder<EnemyContext>();

        idleState = stateBuilder
            .SetName("idle")
            .OnEnter(() => Debug.Log("Entering Idle State"))
            .Build();
        patrolState = stateBuilder
            .SetName("patrol")
            .OnEnter(() => Debug.Log("Entering Patrol State"))
            .Build();
        chaseState = stateBuilder
            .SetName("chase")
            .OnEnter(() => Debug.Log("Enter Chase State"))
            .Build();

        RegisterState("idle", idleState);
        RegisterState("patrol", patrolState);
        RegisterState("chase", chaseState);

        idleState.AddTransition(this)
            .To(patrolState)
            .When(IdleToPatrolFunction)
            .WithAction(() => Debug.Log("Action for Idle to Patrol"))
            .WithPriority(10);
            

        StateMachineRegistry.RegisterStateMachine(this, "EnemyStateMachine");
    }
    bool IdleToPatrolFunction()
    {
        return true;
    }
    
}
