using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

[RequireComponent(typeof(Volume))]
public class LuckHealthVolume : MonoBehaviour
{

    [Inject] private Luck _luck;

    private Volume _volume;
    private Tween _currentTween;

    private void Awake()
    {
        _volume = GetComponent<Volume>();
    }

    private void Start()
    {
        _luck.HealthChanged += OnLuckHealthChanged;
    }

    private void OnDestroy()
    {
        _luck.HealthChanged -= OnLuckHealthChanged;
    }

    private void OnLuckHealthChanged(float health, float healthBefore)
    {
        _currentTween?.Kill();
        var targetWeight = 1 - _luck.NormalizedHealth;
        _currentTween = DOTween.To(() => _volume.weight, value => _volume.weight = value, targetWeight, 0.1f);
    }

}