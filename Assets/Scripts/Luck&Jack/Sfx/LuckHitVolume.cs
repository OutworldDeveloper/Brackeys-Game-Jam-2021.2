using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

[RequireComponent(typeof(Volume))]
public class LuckHitVolume : MonoBehaviour
{

    [Inject] private Luck _luck;

    private Volume _volume;
    private Sequence _currentSequence;

    private void Awake()
    {
        _volume = GetComponent<Volume>();
    }

    private void Start()
    {
        _luck.Damaged += OnLuckDamaged;
    }

    private void OnDestroy()
    {
        _luck.Damaged -= OnLuckDamaged;
    }

    private void OnLuckDamaged(int damage, int health, int healthBefore, FlatVector direction)
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(DOTween.To(() => _volume.weight, value => _volume.weight = value, 1f, 0.05f));
        _currentSequence.Append(DOTween.To(() => _volume.weight, value => _volume.weight = value, 0f, 0.3f));
    }

}