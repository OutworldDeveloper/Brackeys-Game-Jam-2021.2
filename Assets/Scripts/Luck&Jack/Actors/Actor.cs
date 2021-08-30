using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Actor : MonoBehaviour
{

    [SerializeField] private int _startHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _eyesOffset = 1.75f;
    [SerializeField] private Team _team;
    [SerializeField] private UnityEvent _deathEvent;

    public event DamagedEventHandler Damaged;
    public event HealedEventHanndler Healed;
    public event HealthChangedEventHandler HealthChanged;
    public event DiedEventHandler Died;

    public Vector3 EyesPosition => transform.position + Vector3.up * _eyesOffset;
    public int Health { get; private set; }
    public int MaxHealth => _maxHealth;
    public float NormalizedHealth => (float)Health / (float)_maxHealth;
    public Team Team => _team;
    public bool IsDead => Health <= 0;
    public float LastDamage { get; private set; }
    public FlatVector LastDamageDirection { get; private set; }

    protected readonly StateMachine StateMachine = new StateMachine();
    protected State DefaultState { get; private set; }
    protected State DeathState { get; private set; }

    protected virtual void Awake()
    {
        Health = _startHealth;
        DefaultState = CreateDefaultState();
        DeathState = CreateDeathState();
        StateMachine.CreateTransition(DeathState, () => IsDead);
    }

    protected virtual void Start()
    {
        StateMachine.Start(DefaultState);
    }

    protected virtual void Update()
    {
        StateMachine.Tick();
    }

    // If we would ever need to damage objects other than Actors
    // we could just create an Interface with this method
    public virtual bool ApplyDamage(int damage, FlatVector direction)
    {
        if (IsDead)
        {
            return false;
        }

        LastDamage = damage;
        LastDamageDirection = direction;

        var healthBefore = Health;
        Health = Mathf.Max(0, Health - damage);

        Damaged?.Invoke(damage, Health, healthBefore, direction);
        HealthChanged?.Invoke(Health, healthBefore);

        if (IsDead)
        {
            OnDied();
            Died?.Invoke(this);
            _deathEvent.Invoke();
        }

        return true;
    }

    public void ApplyHeal(int heal)
    {
        if (IsDead)
        {
            return;
        }

        var healthBefore = Health;
        Health = Mathf.Min(_maxHealth, Health + heal);
        Healed?.Invoke(heal, Health, healthBefore);
        HealthChanged?.Invoke(Health, healthBefore);
    }

    protected abstract State CreateDefaultState();

    protected abstract State CreateDeathState();

    protected virtual void OnDied() { }

    public delegate void DamagedEventHandler(int damage, int health, int healthBefore, FlatVector direction);
    public delegate void HealedEventHanndler(int heal, int health, int healthBefore);
    public delegate void HealthChangedEventHandler(int health, int healthBefore);
    public delegate void DiedEventHandler(Actor sender);

}