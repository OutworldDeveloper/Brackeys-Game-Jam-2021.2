using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Hides background and windows if needed
public class UI_WindowsManager : MonoBehaviour
{

    [SerializeField] private float _backgroundFadeSpeed = 0.5f;
    [SerializeField] private Color _defaultBackgroundColor;
    [SerializeField] private BackgroundHider _backgroundHider;

    private readonly List<IPanel> _windows = new List<IPanel>();
    private Tween _currentTween;

    private void Start()
    {
        _backgroundHider.Init(_defaultBackgroundColor, _backgroundFadeSpeed);
    }

    public void AddWindow(IPanel window)
    {
        _windows.Add(window);
        window.RectTransform.SetParent(transform, false);
        HideWindowsExceptTheLast();
        UpdateBackground();
    }

    public void RemoveWindow(IPanel window)
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

            window.Deselect();

            if (hide)
            {
                window.Hide();
            }
            else
            {
                hide = window.HideWindowsUnderneath;
                window.Show();
            }
        }

        if (_windows.Count > 0)
        {
            _windows.Last().Select();
        }
    }

    private void UpdateBackground()
    {
        _currentTween?.Kill();

        int windowsCount = _windows.Count;

        if (windowsCount > 0)
        {
            var lastWindow = _windows.Last();
            _backgroundHider.transform.SetSiblingIndex(windowsCount - 1);

            if (lastWindow.HideBackground)
            {
                _backgroundHider.StartHidding(_defaultBackgroundColor);
            }
            else
            {
                _backgroundHider.StopHidding();
            }

            return;
        }

        _backgroundHider.StopHidding();
    }

}