using UnityEngine;

[CreateAssetMenu(fileName = "Gameplay Scene", menuName = "Gameplay Scene")]
public sealed class GameplayScene : ScriptableObject
{

    [SerializeField] private int _environmentIndex;
    [SerializeField] private int _gameplayIndex;

    public int EnvironmentIndex => _environmentIndex;
    public int GameplayIndex => _gameplayIndex; 

}