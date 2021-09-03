using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "Luck And Jack/Map")]
public class LuckMap : Unlockable
{

    [SerializeField] private GameplayScene _gameplayScene;

    public GameplayScene GameplayScene => _gameplayScene; 

}