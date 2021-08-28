using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UI_SelectionMenuButton : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private Text _text;

    public CanvasGroup CanvasGroup { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(string text, Action action)
    {
        _button.onClick.AddListener(action.Invoke);
        _text.text = text;
    }

}