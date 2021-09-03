using UnityEngine;

[CreateAssetMenu(fileName = "Hat", menuName = "Luck And Jack/Hat")]
public class Hat : Unlockable
{

    [SerializeField] private string _description;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private HatVisuals _prefab;

    public string Description => _description;
    public Sprite Sprite => _sprite; 
    public HatVisuals Prefab => _prefab;

}