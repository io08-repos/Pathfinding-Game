using UnityEngine;

public abstract class NPC_State
{
    public abstract void Enter(NPC_StateMachine stateMachine);
    public abstract void Update(NPC_StateMachine stateMachine);
}
