using UnityEngine;

[CreateAssetMenu(fileName = "Gameplay Scene", menuName = "Gameplay Scene")]
public sealed class GameplayScene : ScriptableObject
{

    [SerializeField] private string[] _sceneNames;
    public string[] SceneNames => _sceneNames;

}