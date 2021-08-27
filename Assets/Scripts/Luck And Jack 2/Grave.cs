using System;
using UnityEngine;
using UnityEngine.Events;

public class Grave : Interactable
{

    private const float SavingDuration = 4.3f;
    private const float FullDuration = 8.45f;
    private const string GraveAnimationTrigger = "Extractiion";

    public static event GraveSavedEventHandler GraveSaved;

    [SerializeField] private Animator _animator;
    [SerializeField] private CandlesGroup _candlesGroup;
    [SerializeField] private UnityEvent _savedEvent;

    public override string InteractionText => "extract the soul";
    public override string AnimationTrigger => "grave_saving";
    public bool IsSaved { get; private set; }
    public float LastSavedTime { get; private set; }

    public void Respawn()
    {
        IsSaved = false;
        _animator.SetBool(GraveAnimationTrigger, false);
        _candlesGroup.TurnOn();
    }

    public override bool IsAvaliable()
    {
        return base.IsAvaliable() && !IsSaved;
    }

    protected override void OnInteractionStarted()
    {
        _animator.SetBool(GraveAnimationTrigger, true);
    }

    protected override bool Interact()
    {
        if (Interactor.IsDead)
        {
            _animator.SetBool(GraveAnimationTrigger, false);
            return true;
        }

        if (Time.time > InteractionStartTime + SavingDuration && !IsSaved)
        {
            SaveGrave();
            Interactor.ApplyHeal(33f);
        }

        if (Time.time > InteractionStartTime + FullDuration)
        {
            return true;
        }

        return false;
    }

    private void SaveGrave()
    {
        _candlesGroup.TurnOff();
        IsSaved = true;
        LastSavedTime = Time.time;
        GraveSaved?.Invoke(this);
        _savedEvent?.Invoke();
    }

    public delegate void GraveSavedEventHandler(Grave grave);

}