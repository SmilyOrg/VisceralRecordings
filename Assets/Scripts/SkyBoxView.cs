using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxView : MonoBehaviour
{
    public Material skyBox;

    private Material _fallbackSkybox;

    void Start()
    {
        Init();
    }

    void Update()
    {

    }

    public void ExposureControl(float amount)
    {
        skyBox.SetFloat("_Exposure", amount);
    }

    void Init()
    {
        GatherDefaultData();
    }

    void GatherDefaultData()
    {
        _fallbackSkybox = new Material(skyBox);
    }
}
