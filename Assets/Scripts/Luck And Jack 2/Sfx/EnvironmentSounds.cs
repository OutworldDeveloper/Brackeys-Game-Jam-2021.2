using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundPlayer))]
public class EnvironmentSounds : MonoBehaviour
{

    [SerializeField] private float _minCooldown;
    [SerializeField] private float _maxCooldown;

    private SoundPlayer _soundPlayer;
    private float _nextSoundTime;

    private void Awake()
    {
        _soundPlayer = GetComponent<SoundPlayer>();
    }

    private void Start()
    {
        _nextSoundTime = Time.time + GenerateCooldown();
    }

    private void Update()
    {
        if (Time.time < _nextSoundTime)
            return;
        _nextSoundTime = Time.time + GenerateCooldown();
        _soundPlayer.PlaySound();
    }

    private float GenerateCooldown()
    {
        return Random.Range(_minCooldown, _maxCooldown);
    }

}