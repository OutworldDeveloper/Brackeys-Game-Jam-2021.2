using UnityEngine;

public class LuckGameplaySurvival : LuckGameplayBase
{

    protected override string GetQuestText()
    {
        return $"Souls extracted: {GravesSaved}";
    }

    protected override void OnGraveSaved(Grave grave)
    {
        base.OnGraveSaved(grave);

        if (Graves.Length - GravesSaved > 3)
            return;

        GetFirstSavedGrave().Respawn();
    }

    protected override bool ShouldSpawnGhost()
    {
        return Random.Range(0, 5) == 0;
    }

    private Grave GetFirstSavedGrave()
    {
        Grave targetGrave = null;

        foreach (var grave in Graves)
        {
            if (!grave.IsSaved)
                continue;

            if (targetGrave)
            {
                if (targetGrave.LastSavedTime > grave.LastSavedTime)
                {
                    targetGrave = grave;
                }
                continue;
            }

            targetGrave = grave;
        }

        return targetGrave;
    }

}