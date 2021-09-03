using UnityEngine;

[CreateAssetMenu(fileName = "Hat", menuName = "Luck And Jack/Hat")]
public class Hat : ScriptableObject
{

    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private HatVisuals _prefab;
    [SerializeField] private UnlockRequirements _unlockRequirements;

    public string DisplayName => _displayName;
    public Sprite Sprite => _sprite; 
    public HatVisuals Prefab => _prefab;
    public UnlockRequirements UnlockRequirements => _unlockRequirements;

}

[System.Serializable]
public struct UnlockRequirements
{
    public GameplayScene GameplayScene;
    public bool ShouldWin;
    public int MinGravesSaved;

}