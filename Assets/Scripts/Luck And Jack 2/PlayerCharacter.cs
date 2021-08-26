using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(RotationController))]
public abstract class PlayerCharacter : Actor
{

    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;

    public FlatVector InputVector { get; private set; }
    public float Speed => _speed;
    protected Animator Animator => _animator;

    protected CharacterController CharacterController;
    protected RotationController RotationController;

    public void Move(FlatVector input)
    {
        InputVector = input;
    }

    protected override void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        RotationController = GetComponent<RotationController>();
        base.Awake();
    }

}