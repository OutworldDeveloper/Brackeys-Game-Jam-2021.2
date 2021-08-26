using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{

    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _eyesOffset = 1.75f;
    [SerializeField] private Team _team;

    public event DamagedEventHandler Damaged;
    public event HealedEventHanndler Healed;
    public event HealthChangedEventHandler HealthChanged;
    public event DiedEventHandler Died;

    public Vector3 EyesPosition => transform.position + Vector3.up * _eyesOffset;
    public float Health { get; private set; }
    public float MaxHealth => _maxHealth;
    public float NormalizedHealth => Health / _maxHealth;
    public Team Team => _team;
    public bool IsDead => Health == 0;
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
    public void ApplyDamage(float damage, FlatVector direction)
    {
        if (IsDead)
        {
            return;
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
            Died?.Invoke();
        }
    }

    public void ApplyHeal(float heal)
    {
        if (IsDead)
        {
            return;
        }

        float healthBefore = Health;
        Health = Mathf.Min(_maxHealth, Health + heal);
        Healed?.Invoke(heal, Health, healthBefore);
        HealthChanged?.Invoke(Health, healthBefore);
    }

    protected abstract State CreateDefaultState();

    protected abstract State CreateDeathState();

    protected virtual void OnDied() { }

    public delegate void DamagedEventHandler(float damage, float health, float healthBefore, FlatVector direction);
    public delegate void HealedEventHanndler(float heal, float health, float healthBefore);
    public delegate void HealthChangedEventHandler(float health, float healthBefore);
    public delegate void DiedEventHandler();

}