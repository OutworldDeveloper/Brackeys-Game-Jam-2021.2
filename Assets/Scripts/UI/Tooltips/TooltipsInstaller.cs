using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TooltipsInstaller : MonoInstaller
{

    [SerializeField] private Canvas _canvas;

    public override void InstallBindings()
    {
        BindTooltip<TextTooltip>();
    }

    private void BindTooltip<T>() where T : MonoBehaviour
    {
        Container.Bind<T>().
            FromComponentInNewPrefabResource($"Tooltips/{typeof(T)}").
            UnderTransform(_canvas.transform).
            AsSingle();
    }

}