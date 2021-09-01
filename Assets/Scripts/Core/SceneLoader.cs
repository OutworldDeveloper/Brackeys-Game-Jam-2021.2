using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SceneLoader : MonoBehaviour
{

    [Inject] private ZenjectSceneLoader _sceneLoader;
    [Inject] private TimescaleManager _timescaleManager;
    [SerializeField] private CanvasGroup _background;
    [SerializeField] private Sprite[] _logos;
    [SerializeField] private Image _logoImage;

    private GameplayScene _currentGameplayScene;
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

    private IEnumerator LoadingGameplayScene(GameplayScene nextGameplayScene)
    {
        _isLoading = true;

        _timescaleManager.Pause(this);
        _background.blocksRaycasts = true;

        var hidingTween = _background.DOFade(1f, 0.5f).SetUpdate(true);

        if (!hidingTween.IsComplete())
            yield return null;

        yield return new WaitForSecondsRealtime(1f);

        // Unloading previous scenes
        if (_currentGameplayScene)
        {
            foreach (var sceneName in _currentGameplayScene.SceneNames)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }

        _currentGameplayScene = nextGameplayScene;

        var loadings = new List<AsyncOperation>(nextGameplayScene.SceneNames.Length);

        foreach (var sceneName in nextGameplayScene.SceneNames)
        {
            var loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            loadings.Add(loading);
        }

        // Wait until all the required scenes are loaded
        foreach (var loading in loadings)
        {
            if (!loading.isDone)
            {
                yield return null;
            }
        }

        var showingTween = _background.DOFade(0f, 0.5f).SetDelay(1f).SetUpdate(true);

        if (!showingTween.IsComplete())
            yield return null;

        _background.blocksRaycasts = false;
        _timescaleManager.Unpause(this);

        // This is needed so anything that changed position during the delay would actually move
        Physics.SyncTransforms();

        _isLoading = false;
    }

}