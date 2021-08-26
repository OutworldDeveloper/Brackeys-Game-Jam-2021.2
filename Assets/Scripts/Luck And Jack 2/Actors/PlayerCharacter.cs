using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(RotationController))]
public abstract class PlayerCharacter : Actor
{

    public const string AnimatorSpeedParameter = "speed";

    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;

    public FlatVector DesiredDirection { get; private set; }
    public float Speed => _speed;
    protected Animator Animator => _animator;

    protected CharacterController CharacterController;
    protected RotationController RotationController;

    public void Move(FlatVector direction)
    {
        DesiredDirection = direction.normalized;
    }

    protected override void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        RotationController = GetComponent<RotationController>();
        base.Awake();
    }

}