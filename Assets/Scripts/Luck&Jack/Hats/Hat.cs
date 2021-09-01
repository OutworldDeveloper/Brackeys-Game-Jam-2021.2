using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HatVisuals : MonoBehaviour
{

    private const string Parameter = "_EmissiveExposureWeight";
    
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void StartShining()
    {
        _renderer.sharedMaterial.SetFloat(Parameter, 1f);
    }

    public void StopShining()
    {
        _renderer.sharedMaterial.SetFloat(Parameter, 0f);
    }

}