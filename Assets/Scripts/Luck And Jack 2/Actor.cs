using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{

    protected const string DeathTrigger = "died";

    [SerializeField] private float StartHealth = 100f;
    [SerializeField] private float MaxHealth = 100f;
    [SerializeField] private float EyesOffset = 1.75f;

    public event DamagedEventHandler Damaged;
    public event HealedEventHanndler Healed;
    public event HealthChangedEventHandler HealthChanged;
    public event DiedEventHandler Died;

    public Vector3 EyesPosition => transform.position + Vector3.up * EyesOffset;
    public float Health { get; private set; }
    public bool IsDead => Health > 0;

    protected readonly StateMachine StateMachine = new StateMachine();

    protected virtual void Awake()
    {
        Health = StartHealth;
    }

    protected virtual void Start()
    {
        //StateMachine.AddGlobalTransition(GetDeathState(), () => IsDead);
        //StateMachine.Start();
    }

    public void ApplyDamage(float damage, FlatVector direction)
    {
        if (IsDead)
        {
            return;
        }

        var healthBefore = Health;
        Health = Mathf.Max(0, Health - damage);

        Damaged?.Invoke(damage, Health, healthBefore, direction);
        HealthChanged?.Invoke(Health, healthBefore);

        if (IsDead)
        {
            StateMachine.FireTrigger(DeathTrigger);
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
        Health = Mathf.Min(MaxHealth, Health + heal);
        Healed?.Invoke(heal, Health, healthBefore);
        HealthChanged?.Invoke(Health, healthBefore);
    }

    protected virtual State CreateDefaultState()
    {
        throw new NotImplementedException();
    }

    protected virtual State CreateDeathState()
    {
        throw new NotImplementedException();
    }

    protected virtual void OnDied() { }

    public delegate void DamagedEventHandler(float damage, float health, float healthBefore, FlatVector direction);
    public delegate void HealedEventHanndler(float heal, float health, float healthBefore);
    public delegate void HealthChangedEventHandler(float health, float healthBefore);
    public delegate void DiedEventHandler();

}