using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxView : MonoBehaviour
{
    public Material skyBox;
    public AnimationCurve intensityFade;
    public float intensityDuration;
    public bool applyIntensity = false;

    private float _curTime;
    private float _timeNormalizeValue;

    private Material _fallbackSkybox;

    void Timer()
    {
        _curTime += Time.deltaTime;

        if(applyIntensity)
            EvaluateCurve(_curTime * _timeNormalizeValue);

        if (_curTime >= intensityDuration)
        {
            _curTime = 0f;
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        Timer();
    }

    public void EvaluateCurve(float timeNormalized)
    {
        ExposureControl(intensityFade.Evaluate(timeNormalized));
    }

    public void ExposureControl(float amount)
    {
        skyBox.SetFloat("_Exposure", amount);
    }

    void Init()
    {
        GatherDefaultData();
        _timeNormalizeValue = 1f / intensityDuration;
    }

    public void ResetSkyBox()
    {
        skyBox.CopyPropertiesFromMaterial(_fallbackSkybox);
    }

    void GatherDefaultData()
    {
        _fallbackSkybox = new Material(skyBox);
    }
}
