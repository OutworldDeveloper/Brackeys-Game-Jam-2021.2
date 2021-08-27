using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public class TutorialWall : MonoBehaviour
{

    [Inject] private LuckGameplayTutorial _tutorialGameplay;
    [Inject] private Luck _luck;
    [Inject] private UI_HelperText _helperText;
    [SerializeField] private float _messageDistance = 3.4f;
    [SerializeField] private float _messageDuration = 0.5f;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _tutorialGameplay.JackSaved += OnJackSaved;
    }

    private void Update()
    {
        var distance = FlatVector.Distance((FlatVector)_luck.transform.position, (FlatVector)transform.position);
        if (distance < _messageDistance)
        {
            _helperText.Show("You need to wake up Jack first.", _messageDuration);
        }
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