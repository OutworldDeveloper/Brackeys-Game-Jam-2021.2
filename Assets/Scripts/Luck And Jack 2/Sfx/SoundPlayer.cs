using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{

    [SerializeField] private float _minPitch = 1f;
    [SerializeField] private float _maxPitch = 1f;
    [SerializeField] private AudioClip[] _audioClips;

    public bool IsPlaying => _audioSource.isPlaying;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        _audioSource.clip = _audioClips[Random.Range(0, _audioClips.Length)];
        _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
        _audioSource.Play();
    }

}