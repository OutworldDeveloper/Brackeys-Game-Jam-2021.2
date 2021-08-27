using UnityEngine;

public class GoingToInteractableState : State
{

    private const float CancelingDelay = 0.3f;

    public bool HasReachedTarget => _luck.transform.GetFlatPosition() == _luck.TargetInteractable.Point;
    public bool CanBeCanceled => Time.time - _startTime > CancelingDelay;

    private readonly Luck _luck;
    private readonly CharacterController _characterController;
    private readonly RotationController _rotationController;
    private readonly Animator _animator;

    private float _startTime;

    public GoingToInteractableState(Luck luck, CharacterController characterController, RotationController rotationController, Animator animator)
    {
        _luck = luck;
        _characterController = characterController;
        _rotationController = rotationController;
        _animator = animator;
    }

    public override void Start()
    {
        _characterController.enabled = false;
        _startTime = Time.time;
    }

    public override void Tick()
    {
        var targetPosition = _luck.TargetInteractable.Point;
        _luck.transform.position = (FlatVector)Vector3.MoveTowards(_luck.transform.GetFlatPosition(), targetPosition, _luck.Speed * Time.deltaTime);
        var direction = (targetPosition - _luck.transform.position).normalized;
        _rotationController.LookIn(direction);
        _animator.SetFloat(PlayerCharacter.AnimatorSpeedParameter, _luck.Speed);
    }

    public override void End()
    {
        _animator.SetFloat(PlayerCharacter.AnimatorSpeedParameter, 0f);
        _characterController.enabled = true;
    }

}
