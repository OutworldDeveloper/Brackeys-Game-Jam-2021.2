using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_TimescalePresetButton : MonoBehaviour
{

    [SerializeField] private Text text = default;

    public void Setup(float targetTimescale, Action<float> pressedCallback)
    {
        text.text = $"{targetTimescale}x";
        GetComponent<Button>().onClick.AddListener(() => pressedCallback.Invoke(targetTimescale));
    }

}