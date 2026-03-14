using UnityEngine;

public enum State
{
    Idle,
    Patrol,
    Chase,

    MaxValue
}

public class NPC_StateMachine : MonoBehaviour
{
    protected NPC_State[] States;

    private NPC_State _currentState;
    public NPC_State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            _currentState.Enter(this);
        }
    }

    protected void InitializeStates() => States = new NPC_State[(int)State.MaxValue];
    public void ChangeState(State value) => CurrentState = States[(int)value];

    private void Update() => CurrentState.Update(this);
}
