using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SleepingJack : Interactable
{

    private const float SavingDuratioon = 4.96f;
 
    public event Action JackSaved;

    [SerializeField] private UnityEvent _savedEvent;

    public override string InteractionText => "wake up Jack";
    public override string AnimationTrigger => "jack_saving";

    private Jack _jack;

    public void PlaceJack(Jack jack)
    {
        _jack = jack;
        _jack.transform.position = transform.position;
        _jack.transform.rotation = transform.rotation;
        _jack.Sleep();
    }

    public override bool IsAvaliable()
    {
        return base.IsAvaliable() && _jack;
    }

    protected override bool Interact()
    {
        if (Time.time < InteractionStartTime + SavingDuratioon)
        {
            return false;
        }

        _jack.WakeUp();
        _jack = null;
        JackSaved?.Invoke();
        _savedEvent.Invoke();
        return true;
    }

}