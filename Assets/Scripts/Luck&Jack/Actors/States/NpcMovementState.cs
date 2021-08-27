using UnityEngine;
using UnityEngine.AI;

public class NpcMovementState : State
{

    private BaseNpc _npc;
    private NavMeshAgent _agent;
    private Animator _animator;
    private RotationController _rotationController;

    public NpcMovementState(BaseNpc npc, NavMeshAgent agent, RotationController rotationController, Animator animator)
    {
        _npc = npc;
        _agent = agent;
        _animator = animator;
        _rotationController = rotationController;
    }

    public override void Start()
    {
        _agent.speed = _npc.Speed;
    }

    public override void Tick()
    {
        _rotationController.LookAt((FlatVector)_agent.steeringTarget);
    }

    public override void End()
    {
        _agent.speed = 0f;
    }

}