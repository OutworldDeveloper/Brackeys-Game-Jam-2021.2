using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

// 3 режима полоски здоровья - над лак, на экране и без
public class UI_LuckHud : UI_BaseHud
{

    [Inject] private IEnumerable<IInteractable> _interactables;
    [Inject] private PlayerPawn _playerPawn;

    [SerializeField] private Text _interactionsText;
    [SerializeField] private CanvasGroup _interactionsTextCanvasGroup;
    [SerializeField] private Setting_KeyCode _keyCodeSetting;
    
    private Camera _camera;
    private Vector3 _lastInteractionsTextPosition;

    protected override void OnHudStarted()
    {
        base.OnHudStarted();
        _camera = _playerPawn.PlayerController.PlayerCamera;
        _interactionsTextCanvasGroup.alpha = 0f;
    }

    private void Update()
    {
        IInteractable targetInteractable = null;

        foreach (var interactable in _interactables)
        {
            if (!interactable.IsAvaliable())
                continue;
            if (FlatVector.Distance(_playerPawn.Luck.transform.position, interactable.RangeCenterPoint) < interactable.Range)
            {
                targetInteractable = interactable;
                break;
            }
        }

        if (targetInteractable != null)
        {
            _interactionsText.text = $"Press {_keyCodeSetting.GetValue()} to {targetInteractable.InteractionText}";
            _lastInteractionsTextPosition = targetInteractable.TextPosition;

            if (_interactionsTextCanvasGroup.alpha < 1f)
                _interactionsTextCanvasGroup.alpha += Time.unscaledDeltaTime * 3f;
        }
        else
        {
            if (_interactionsTextCanvasGroup.alpha > 0f)
                _interactionsTextCanvasGroup.alpha -= Time.unscaledDeltaTime * 3f;
        }

        _interactionsText.transform.position = _camera.WorldToScreenPoint(_lastInteractionsTextPosition);
    }

    public class Factory : PlaceholderFactory<PlayerPawn, UI_LuckHud> { }

}