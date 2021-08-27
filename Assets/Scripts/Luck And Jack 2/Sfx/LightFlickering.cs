using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(HDAdditionalLightData))]
public class LightFlickering : MonoBehaviour
{

    [SerializeField] private float intensityChangingSpeed = 70000f;
    [SerializeField] private float changeIntervalDelayMin = 0.2f;
    [SerializeField] private float changeIntervalDelayMax = 1f;
    [SerializeField] private float minIntensity = 50000f;
    [SerializeField] private float maxIntensity = 70000f;

    private HDAdditionalLightData lightData;

    private float nextChangeTime;
    private float targetIntensity;

    private void Awake()
    {
        lightData = GetComponent<HDAdditionalLightData>();
    }

    private void Start()
    {
        targetIntensity = lightData.intensity;
        nextChangeTime = Time.time + Random.Range(changeIntervalDelayMin, changeIntervalDelayMax);
    }

    private void Update()
    {
        float newIntensity = Mathf.MoveTowards(lightData.intensity, targetIntensity, Time.deltaTime * intensityChangingSpeed);
        lightData.SetIntensity(newIntensity, LightUnit.Lumen);

        if (Time.time > nextChangeTime)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            nextChangeTime = Time.time + Random.Range(changeIntervalDelayMin, changeIntervalDelayMax);
        }
       
    }

}
