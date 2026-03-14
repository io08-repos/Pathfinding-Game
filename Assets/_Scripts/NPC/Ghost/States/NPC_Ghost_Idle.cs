using UnityEngine;

public class NPC_Ghost_Idle : NPC_State
{
    public override void Enter(NPC_StateMachine stateMachine)
    {
        NPC_Ghost ghost = (NPC_Ghost) stateMachine;
        ghost.IdleTimer = (float) Random.Range(NPC_Ghost.MIN_IDLE_TIME, NPC_Ghost.MAX_IDLE_TIME) / 1000;
    }

    public override void Update(NPC_StateMachine stateMachine)
    {
        NPC_Ghost ghost = (NPC_Ghost) stateMachine;
        
        if (ghost.IdleTimer < 0)
        {
            ghost.ChangeState(State.Patrol);
            return;
        }

        ghost.IdleTimer -= Time.deltaTime;
    }
}
