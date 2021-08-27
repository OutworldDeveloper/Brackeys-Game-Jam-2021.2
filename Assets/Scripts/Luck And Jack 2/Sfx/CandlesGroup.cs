using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlesGroup : MonoBehaviour
{

    [SerializeField] private Light _light;
    [SerializeField] private Candle[] _candles;
    [SerializeField] private bool _findInChildren;

    private void Awake()
    {
        if (_findInChildren)
        {
            _candles = GetComponentsInChildren<Candle>();
        }
    }

    public void TurnOn()
    {
        Change(true);
    }

    public void TurnOff()
    {
        Change(false);
    }

    private void Change(bool b)
    {
        _light.enabled = b;
        foreach (var candle in _candles)
        {
            candle.EnableGlowing(b);
        }
    }

}