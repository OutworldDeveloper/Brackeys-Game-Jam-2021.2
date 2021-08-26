using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rat))]
public class RatBrain : MonoBehaviour
{

    [Inject] private Luck _luck;
    [SerializeField] private float _jumpDistance = 5f;
    [SerializeField] private float _attackCooldownMin = 4f;
    [SerializeField] private float _attackCoooldownMax = 13f;

    private Rat _rat;
    private bool _isAttacking;
    private float _nextAttackTime;

    private void Awake()
    {
        _rat = GetComponent<Rat>();
    }

    private void Start()
    {
        _rat.Died += () => Destroy(this);
    }

    private void Update()
    {
        if (_isAttacking)
        {
            if (_luck.IsDead)
            {
                _isAttacking = false;
            }

            var targetPosition = _luck.transform.GetFlatPosition();
            _rat.MoveTo(targetPosition);

            if (FlatVector.Distance((FlatVector)_rat.transform.position, targetPosition) < _jumpDistance)
            {
                var direction = ((FlatVector)_luck.transform.position - (FlatVector)_rat.transform.position).normalized;
                _rat.Jump(direction);
                _isAttacking = false;
                _nextAttackTime = Time.time + Random.Range(_attackCooldownMin, _attackCoooldownMax);
            }
        }
        else
        {
            if (Time.time > _nextAttackTime && !_luck.IsDead)
            {
                _isAttacking = true;
            }


        }
    }

}