using UnityEngine;

public class NPC_Ghost_Patrol : NPC_State
{
    public override void Enter(NPC_StateMachine stateMachine)
    {
        NPC_Ghost ghost = (NPC_Ghost) stateMachine;

        ghost.TargetNode = AStarManager.Instance.RandomTargetNode(ghost.CurrentNode);
        ghost.Path = AStarManager.Instance.FindPath(ghost.CurrentNode, ghost.TargetNode);
        ghost.NextNode = ghost.Path[^1];
    }

    public override void Update(NPC_StateMachine stateMachine)
    {
        NPC_Ghost ghost = (NPC_Ghost) stateMachine;

        var newPosition = Vector3.MoveTowards(ghost.transform.position, ghost.NextNode.transform.position, ghost.MoveSpeed * Time.deltaTime);
        ghost.transform.position = newPosition;

        var distanceLeft = Vector3.Distance(ghost.transform.position, ghost.NextNode.transform.position);
        if (distanceLeft < 0.01f)
        {
            ghost.CurrentNode = ghost.NextNode;

            if (ghost.CurrentNode == ghost.TargetNode)
            {
                ghost.ChangeState(State.Idle);
                return;
            }

            ghost.Path.RemoveAt(ghost.Path.Count - 1);
            ghost.NextNode = ghost.Path[^1];
        }
    }
}
