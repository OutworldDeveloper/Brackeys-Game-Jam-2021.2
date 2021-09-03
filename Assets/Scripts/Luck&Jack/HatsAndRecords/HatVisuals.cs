using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatVisuals : MonoBehaviour
{

    private const string Parameter = "_EmissiveExposureWeight";

    [SerializeField] private Renderer _renderer;

    private void Awake()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        StopShining();
    }

    public void StartShining()
    {
        _renderer.sharedMaterial.SetFloat(Parameter, 0f);
    }

    public void StopShining()
    {
        _renderer.sharedMaterial.SetFloat(Parameter, 1f);
    }

}