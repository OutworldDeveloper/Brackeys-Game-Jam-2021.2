using System;
using UnityEngine;
using Zenject;

public class GameplayController : MonoBehaviour
{

    [Inject] private PlayerController _playerController;
    [Inject] private IConsole _console;

    public PlayerController PlayerController => _playerController;
    protected IConsole Console => _console;

    protected virtual void Start()
    {
        _console.Log($"GameplayController of type {GetType()} is initialized.");
    }

}