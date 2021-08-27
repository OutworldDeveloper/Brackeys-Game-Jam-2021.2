using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public class TutorialWall : MonoBehaviour
{

    [Inject] private LuckGameplayTutorial _tutorialGameplay;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _tutorialGameplay.JackSaved += OnJackSaved;
    }

    private void OnDestroy()
    {
        _tutorialGameplay.JackSaved -= OnJackSaved;
    }

    private void OnJackSaved()
    {
        _collider.enabled = false;
        Destroy(this);
    }

}