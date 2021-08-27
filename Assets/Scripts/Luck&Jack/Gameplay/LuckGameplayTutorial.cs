using System;
using Zenject;

public class LuckGameplayTutorial : LuckGameplayBase
{

    public event Action JackSaved;

    [Inject] private SleepingJack _sleepingJack;

    protected override void Start()
    {
        base.Start();
        _sleepingJack.PlaceJack(Jack);
        _sleepingJack.JackSaved += OnJackSaved;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // What if we already unsubscribed?
        _sleepingJack.JackSaved -= OnJackSaved;
    }

    private void OnJackSaved()
    {
        _sleepingJack.JackSaved -= OnJackSaved;
        JackSaved?.Invoke();
    }

}