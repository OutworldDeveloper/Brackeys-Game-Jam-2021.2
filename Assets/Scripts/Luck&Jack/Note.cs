using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Note : MonoBehaviour, IInteractable
{

    [Inject] private UI_YesNoWindow.Factory _factory;

    public bool IsInstant => true;
    public FlatVector RangeCenterPoint => (FlatVector)transform.position;
    public float Range => 3f;
    public Vector3 TextPosition => (FlatVector)transform.position;
    public string InteractionText => "where am i";

    private bool _isTaken;

    public bool IsAvaliable()
    {
        return !_isTaken;
    }

    public void StartInteraction(Luck luck)
    {
        var testWindow = _factory.Create();
        testWindow.SetTitle("You just picked up a trash!");
    }

}