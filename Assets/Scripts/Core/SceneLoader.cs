using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

// TODO: Rename
public class SceneLoader : MonoBehaviour
{

    [Inject] private TimescaleManager _timescaleManager;
    [SerializeField] private CanvasGroup _background;
    [SerializeField] private Sprite[] _logos;
    [SerializeField] private Image _logoImage;
    [SerializeField] private Text _loadingText;

    public GameplayScene LoadedGameplayScene { get; private set; }
    private bool _isLoading;

    private void Start()
    {
        _background.blocksRaycasts = false;
        _background.alpha = 0f;
    }

    public void LoadGameplayScene(GameplayScene gameplayScene)
    {
        if (_isLoading)
        {
            throw new Exception("Tried to load a new gameplay scene while another is currently being loaded.");
        }
        StartCoroutine(LoadingGameplayScene(gameplayScene));
    }

    public void ReloadCurrentGameplayScene()
    {
        if(LoadedGameplayScene == null)
        {
            throw new InvalidOperationException("Tried to restart current gameplay scene, but there is no gameplay scene loaded.");
        }
        LoadGameplayScene(LoadedGameplayScene);
    }

    private IEnumerator LoadingGameplayScene(GameplayScene nextGameplayScene)
    {
        _isLoading = true;

        _loadingText.text = $"Loading {nextGameplayScene.DisplayName}";

        _timescaleManager.Pause(this);
        _background.blocksRaycasts = true;

        _background.DOFade(1f, 0.5f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1f);

        // Unloading previous scenes
        if (LoadedGameplayScene)
        {
            foreach (var sceneName in LoadedGameplayScene.SceneNames)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        LoadedGameplayScene = nextGameplayScene;

        var loadings = new List<AsyncOperation>(nextGameplayScene.SceneNames.Length);

        foreach (var sceneName in nextGameplayScene.SceneNames)
        {
            var loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            loadings.Add(loading);
        }

        // Wait until all the required scenes are loaded
        yield return new WaitUntil(() =>
        {
            foreach (var loading in loadings)
            {
                if (!loading.isDone)
                {
                    return false;
                }
            }
            return true;
        });

        _background.DOFade(0f, 0.5f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.5f);

        _background.blocksRaycasts = false;
        _timescaleManager.Unpause(this);

        //Physics.SyncTransforms();

        _isLoading = false;
    }

}