using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

// This is needed to decouple ProjectContext and GameContext
// Project context will have common things like SceneLoader, Windows etc.
// GameContext will have game specific things and potentially will be swappable at runtime.
public class Bootstrap : MonoBehaviour
{

    [Inject] private SceneLoader _sceneLoader;
    [SerializeField] private string _contextSceneName;
    [SerializeField] private GameplayScene _initialScene;

    private void Start()
    {
        if (_contextSceneName != string.Empty)
        {
            SceneManager.LoadScene(_contextSceneName);
        }
        _sceneLoader.LoadGameplayScene(_initialScene);
    }

}