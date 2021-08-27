using System;
using Zenject;

public class LuckGameplayTutorial : LuckGameplayBase
{

    public event Action JackSaved;

    [Inject] private SleepingJack _sleepingJack;
    [Inject] private UI_HelperText _helperText;

    protected override bool IgnoreDistanceSleeping => !_hasSavedJack;

    private bool _hasSavedJack;

    protected override void Start()
    {
        base.Start();
        _sleepingJack.PlaceJack(Jack);
        _sleepingJack.JackSaved += OnJackSaved;

        _helperText.Show("Hello and welcome back!", 10f);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // What if we already unsubscribed?
        _sleepingJack.JackSaved -= OnJackSaved;
    }

    private void OnJackSaved()
    {
        _hasSavedJack = true;
        _sleepingJack.JackSaved -= OnJackSaved;
        JackSaved?.Invoke();
    }

}