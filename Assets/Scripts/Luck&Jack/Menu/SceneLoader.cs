using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    [Inject] private ZenjectSceneLoader _sceneLoader;
    [Inject] private TimescaleManager _timescaleManager;
    [SerializeField] private CanvasGroup _background;
    [SerializeField] private Sprite[] _logos;
    [SerializeField] private Image _logoImage;

    private void Start()
    {
        _background.blocksRaycasts = false;
        _background.alpha = 0f;
    }

    public void LoadScene(int index)
    {
        LoadScene(index, (container) => { });
    }

    public void LoadScene(int index, Action<DiContainer> extraBindings)
    {
        _background.blocksRaycasts = true;
        _timescaleManager.Pause(this);

        _logoImage.sprite = _logos[UnityEngine.Random.Range(0, _logos.Length)];

        _background.DOFade(1f, 0.5f).
            SetUpdate(true).
            OnComplete(() =>
            {
                var loadingOperation = _sceneLoader.LoadSceneAsync(index, LoadSceneMode.Single, extraBindings);
                loadingOperation.completed += OnLevelLoaded;
            });
    }

    private void OnLevelLoaded(AsyncOperation obj)
    {
        _background.DOFade(0f, 0.5f).
            SetDelay(1f).
            SetUpdate(true).
            OnComplete(() =>
            {
                _background.blocksRaycasts = false;
                _timescaleManager.Unpause(this);
                Physics.SyncTransforms(); // This is needed so anything that changed position during the delay would actually move
            });
    }

}