using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UI_SelectionMenu : UI_BaseWindow
{

    [SerializeField] private Button _prefabButton = default;

    private readonly List<Button> _currentButtons = new List<Button>();

    public void AddSelection(string name, Action action)
    {
        var button = Instantiate(_prefabButton, transform);
        button.GetComponentInChildren<Text>().text = name;
        button.onClick.AddListener(() => action.Invoke());
        _currentButtons.Add(button);
    }

    protected override Sequence CreateOpeningSequence()
    {
        var mainSequence = base.CreateOpeningSequence();
        var buttonsSequence = DOTween.Sequence();

        for (int i = 0; i < _currentButtons.Count; i++)
        {
            var button = _currentButtons[i];

            buttonsSequence.Insert(i * 0.05f, 
                button.transform.DOScale(Vector3.one, 0.1f).
                From(Vector2.zero));

            buttonsSequence.Insert(i * 0.05f, 
                button.image.DOFade(1f, 0.1f).
                From(0f));
        }

        mainSequence.Insert(0, buttonsSequence);
        return mainSequence;
    }

    public class Factory : PlaceholderFactory<UI_SelectionMenu> { }

}