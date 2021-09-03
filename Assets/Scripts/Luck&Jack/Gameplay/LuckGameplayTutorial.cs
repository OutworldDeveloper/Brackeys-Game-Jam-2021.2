using System;
using UnityEngine;
using Zenject;

public class LuckGameplayTutorial : LuckGameplayBase
{

    public event Action JackSaved;

    [Inject] private SleepingJack _sleepingJack;
    [Inject] private TutorialWall _tutorialWall;
    [SerializeField] private Ghost.GhostSettings _tutorialGhost;

    protected override bool IgnoreDistanceSleeping => !_hasSavedJack;

    private bool _hasSavedJack;

    protected override void Start()
    {
        base.Start();
        _sleepingJack.PlaceJack(Jack);
        _sleepingJack.JackSaved += OnJackSaved;
        _tutorialWall.Enable();
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
        _tutorialWall.Disable();
        UpdateQuest();
    }

    protected override void OnGraveSaved(Grave grave)
    {
        base.OnGraveSaved(grave);

        if (GravesSaved == Graves.Length)
        {
            Win();

            var menu = SelectionMenuFactory.Create();

            menu.SetTitle("You have completed the tutorial");
            menu.SetDescription("Survival mode unlocked");
            menu.DisallowClosing();
            menu.AddSelection("Survival Mode", () => Debug.Log("Survival Mode"));
            menu.AddSelection("Main Menu", () => Debug.Log("Main Menu"));
        }
    }

    protected override bool ShouldSpawnGhost()
    {
        return GravesSaved == 2 || GravesSaved == 4;
    }

    protected override string GetQuestText()
    {
        if (_hasSavedJack)
        {
            return $"Extract souls {GravesSaved} / {Graves.Length}";
        }

        return "Find and wake up Jack";
    }

}
