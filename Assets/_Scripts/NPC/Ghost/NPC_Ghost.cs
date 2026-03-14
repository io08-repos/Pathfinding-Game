using System.Collections.Generic;

using UnityEngine;

public class NPC_Ghost : NPC_StateMachine
{
    public bool PathFinished = true;

    public float IdleTimer;
    public const int MIN_IDLE_TIME = 1000;
    public const int MAX_IDLE_TIME = 2001;

    public float MoveSpeed;

    public Node CurrentNode { get; set; }
    public Node TargetNode { get; set; }
    public Node NextNode { get; set; }
    public List<Node> Path { get; set; }

    private void Start()
    {
        InitializeStates();

        States[(int)State.Idle] = new NPC_Ghost_Idle();
        States[(int)State.Patrol] = new NPC_Ghost_Patrol();
        States[(int)State.Chase] = new NPC_Ghost_Chase();

        CurrentNode = AStarManager.Instance.RandomNode();
        transform.position = CurrentNode.transform.position;

        ChangeState(State.Idle);
    }
}
