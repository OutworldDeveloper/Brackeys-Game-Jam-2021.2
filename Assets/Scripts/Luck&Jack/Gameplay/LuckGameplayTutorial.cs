using System;
using UnityEngine;
using Zenject;

public class LuckGameplayTutorial : LuckGameplayBase
{

    public event Action JackSaved;

    [Inject] private SleepingJack _sleepingJack;
    [Inject] private UI_SelectionMenu.Factory _selectionMenuFactory;
    [SerializeField] private Ghost.GhostSettings _tutorialGhost;

    protected override bool IgnoreDistanceSleeping => !_hasSavedJack;

    private bool _hasSavedJack;

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
        _hasSavedJack = true;
        _sleepingJack.JackSaved -= OnJackSaved;
        JackSaved?.Invoke();
    }

    protected override void OnGraveSaved(Grave grave)
    {
        base.OnGraveSaved(grave);

        if (GravesSaved == 2 || GravesSaved == 4)
        {
            SpawnGhost(_tutorialGhost);
        }

        if (GravesSaved == Graves.Length)
        {
            var menu = _selectionMenuFactory.Create();

            menu.SetTitle("You have completed the tutorial");
            menu.SetDescription("Survival mode unlocked");
            menu.DisableClosing();
            menu.AddSelection("Survival Mode", () => Debug.Log("Survival Mode"));
            menu.AddSelection("Main Menu", () => Debug.Log("Main Menu"));
        }
    }

}