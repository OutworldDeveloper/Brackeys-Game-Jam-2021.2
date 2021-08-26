using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : Interactable
{

    public override string InteractionText => "extract the soul";
    public override string AnimationTrigger => "grave_saving";
    public bool IsSaved { get; private set; }
    public float LastSavedTime { get; private set; }

    public override bool IsAvaliable()
    {
        return base.IsAvaliable() && !IsSaved;
    }

    protected override void OnInteractionStarted()
    {
        GetComponent<Animator>().SetBool("Extractiion", true);
    }

    protected override bool Interact()
    {
        if (Interactor.IsDead)
        {
            GetComponent<Animator>().SetBool("Extractiion", false);
            return true;
        }

        if (Time.time > InteractionStartTime + 4.3f && !IsSaved)
        {
            LastSavedTime = Time.time;
            IsSaved = true;
            Interactor.ApplyHeal(33f);
            Debug.Log("Saved!");
        }

        if (Time.time > InteractionStartTime + 8.45f)
        {
            Debug.Log("Finished!");
            return true;
        }

        return false;
    }

}