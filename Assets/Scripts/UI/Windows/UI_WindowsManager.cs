using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Hides background and windows if needed
public class UI_WindowsManager : MonoBehaviour
{

    [SerializeField] private bool _hideWindows;
    [SerializeField] private float _backgroundFadeDuration = 0.5f;
    [SerializeField] private CanvasGroup _background;

    private readonly List<UI_BaseWindow> _windows = new List<UI_BaseWindow>();
    private Tween _currentTween;

    private void Start()
    {
        _background.alpha = 0f;
        _background.interactable = false;
        _background.gameObject.SetActive(false);
    }

    public void AddWindow(UI_BaseWindow window)
    {
        _windows.Add(window);
        HideWindowsExceptTheLast();
        UpdateBackground();
    }

    public void RemoveWindow(UI_BaseWindow window)
    {
        _windows.Remove(window);
        HideWindowsExceptTheLast();
        UpdateBackground();
    }

    private void HideWindowsExceptTheLast()
    {
        bool hide = false;

        for (int i = _windows.Count - 1; i >= 0; i--)
        {
            var window = _windows[i];

            if (hide)
            {
                window.Hide();
            }
            else
            {
                hide = window.HideWindowsUnderneath;
                window.Show ();
            }
        }
    }

    private void UpdateBackground()
    {
        _currentTween?.Kill();

        int windowsCount = _windows.Count;

        if (windowsCount > 0)
        {
            _background.transform.SetSiblingIndex(windowsCount - 1);
            _background.gameObject.SetActive(true);
            _background.blocksRaycasts = true;
            _background.interactable = true;

            _currentTween = _background.DOFade(1f, _backgroundFadeDuration).SetSpeedBased().SetUpdate(true);
            return;
        }

        _background.blocksRaycasts = false;
        _background.interactable = false;
        
        _currentTween = _background.DOFade(0f, _backgroundFadeDuration).SetSpeedBased().OnComplete(() => _background.gameObject.SetActive(false)).SetUpdate(true);
    }

}