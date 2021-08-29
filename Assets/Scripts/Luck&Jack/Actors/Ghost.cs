using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(RotationController))]
public class Ghost : MonoBehaviour
{

    public static GhostSettings DefaultSettings => new GhostSettings()
    {
        Speed = 4.5f,
        AttackingSpeed = 8f,
        RunningAwaySpeed = 12f,
        LifetimeMin = 10f,
        LifetimeMax = 15f,
    };

    [SerializeField] private SoundPlayer _soundPlayerDamage;
    [SerializeField] private SoundPlayer _soundPlayerPiss;
    [SerializeField] private float _maxTimeInLight = 0.15f;
    [Inject] public GhostSettings Settings { get; private set; }
    [Inject] private Luck _luck;
    [Inject] private Jack _jack;

    private RotationController _rotationController;
    private readonly StateMachine _stateMachine = new StateMachine();
    private float _luckDistance => GetDistanceTo(_luck.transform);
    private float _jackDistance => GetDistanceTo(_jack.transform);

    private float _deathTime;
    private float _timeInLight;

    private void Awake()
    {
        _rotationController = GetComponent<RotationController>();
    }

    private void Start()
    {
        var chasing = new Chasing(this, Settings.Speed);
        var runningAway = new RunningAway(this, Settings.Speed, false);
        var attacking = new Chasing(this, Settings.AttackingSpeed);
        var escaping = new RunningAway(this, Settings.RunningAwaySpeed, true);

        _stateMachine.CreateTransition(chasing, attacking, () => _luckDistance < 3.5f);
        _stateMachine.CreateTransition(chasing, runningAway, () => _timeInLight > _maxTimeInLight);
        _stateMachine.CreateTransition(chasing, escaping, () => chasing.Ended);

        _stateMachine.CreateTransition(attacking, chasing, () => _luckDistance > 4f);
        _stateMachine.CreateTransition(attacking, runningAway, () => _timeInLight > _maxTimeInLight);
        _stateMachine.CreateTransition(attacking, escaping, () => attacking.Ended);

        _stateMachine.CreateTransition(runningAway, chasing, () => !IsJackLooking() && runningAway.Ended);

        _stateMachine.CreateTransition(escaping, () => Time.time > _deathTime);
        _stateMachine.CreateTransition(escaping, () => _luck.IsDead);

        _stateMachine.Start(chasing);

        _deathTime = Time.time + Random.Range(Settings.LifetimeMin, Settings.LifetimeMax);
        _soundPlayerPiss.PlaySound();
    }

    private void Update()
    {
        if (IsJackLooking())
        {
            _timeInLight += Time.deltaTime;
        }
        else
        {
            _timeInLight = 0f;
        }

        _stateMachine.Tick();
    }

    private float GetDistanceTo(Transform target)
    {
        return FlatVector.Distance(target.position, transform.position);
    }

    private bool IsJackLooking()
    {
        return _jack.IsInLight(transform);
    }

    [System.Serializable]
    public struct GhostSettings
    {
        public float Speed;
        public float AttackingSpeed;
        public float RunningAwaySpeed;
        public float LifetimeMin;
        public float LifetimeMax;
    }

    public class Factory : PlaceholderFactory<GhostSettings, Ghost> { }

    private class Chasing : State
    {

        public bool Ended { get; private set; }

        private readonly Ghost _ghost;
        private readonly RotationController _rotationController;
        private readonly SoundPlayer _soundPlayer;
        private readonly float _speed;
        private readonly Actor _target;

        private bool _isSprinting;

        public Chasing(Ghost ghost, float speed)
        {
            _ghost = ghost;
            _rotationController = ghost._rotationController;
            _soundPlayer = _ghost._soundPlayerDamage;
            _speed = speed;
            _target = ghost._luck;
        }

        public override void Tick()
        {
            var distance = _ghost._luckDistance;

            if (_isSprinting && distance < 7.5f)
            {
                _isSprinting = false;
            }

            if (!_isSprinting && distance > 10f)
            {
                _isSprinting = true;
            }

            float currentSpeed = _isSprinting ? 20f : _speed;

            _ghost.transform.position = Vector3.MoveTowards(_ghost.transform.position, _target.transform.position, currentSpeed * Time.deltaTime);

            _rotationController.LookAt((FlatVector)_target.transform.position);

            if (distance < 0.25f)
            {
                _target.ApplyDamage(1, (FlatVector)_ghost.transform.forward);
                _soundPlayer.PlaySound();
                Ended = true;
            }
        }

    }

    private class RunningAway : State
    {
        public bool Ended => Time.time > _endTime;

        private readonly Ghost _ghost;
        private readonly float _speed;
        private readonly RotationController _rotationController;
        private readonly bool _destroy;
        private SoundPlayer _soundPlayer;

        private float _endTime;

        public RunningAway(Ghost ghost, float speed, bool destroy)
        {
            _ghost = ghost;
            _speed = speed;
            _rotationController = ghost._rotationController;
            _destroy = destroy;
            _soundPlayer = _ghost._soundPlayerPiss;
        }

        public override void Start()
        {
            if (_destroy)
            {
                Destroy(_ghost.gameObject, 15f);
            }

            _soundPlayer.PlaySound();

            _endTime = Time.time + Random.Range(0.5f, 4f);
        }

        public override void Tick()
        {
            var jack = _ghost._jack;

            var direction = (FlatVector)(jack.transform.position - _ghost.transform.position).normalized;
            _rotationController.LookAt(-direction);
            _ghost.transform.position -= (jack.transform.position - _ghost.transform.position).normalized * _speed * Time.deltaTime;
        }

        public override void End()
        {
            if (!_soundPlayer.IsPlaying)
                _soundPlayer.PlaySound();
        }

    }

}