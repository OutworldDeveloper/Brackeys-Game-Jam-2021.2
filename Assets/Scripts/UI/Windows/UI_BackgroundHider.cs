using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BackgroundHider : MonoBehaviour
{ 

    [SerializeField] private CanvasGroup _background = default;

    private HashSet<object> _requests = new HashSet<object>();
    private Tween _currentTween;

    private void Start()
    {
        _background.alpha = 0f;
        _background.interactable = false;
        _background.gameObject.SetActive(false);
    }

    public void RequestHidding(object requester)
    {
        _requests.Add(requester);
        UpdateBackground();
    }

    public void CancelRequest(object requester)
    {
        _requests.Remove(requester);
        UpdateBackground();
    }

    private void UpdateBackground()
    {
        _currentTween?.Kill();

        if (_requests.Count > 0)
        {
            _background.blocksRaycasts = true;
            _background.gameObject.SetActive(true);
            _background.transform.SetSiblingIndex(_requests.Count - 1);

            _background.interactable = true;
            if (_background.alpha < 1)
            {
                _currentTween = _background.DOFade(1f, 0.5f / (1 - _background.alpha)).
                    SetUpdate(true);
            }
            return;
        }

        _background.interactable = false;
        if (_background.alpha > 0)
        {
            _background.blocksRaycasts = false;
            _currentTween = _background.DOFade(0f, 0.5f * _background.alpha).
                OnComplete(() => _background.gameObject.SetActive(false)).
                SetUpdate(true);
        }
    }

}