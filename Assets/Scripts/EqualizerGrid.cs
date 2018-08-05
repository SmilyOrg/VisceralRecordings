using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EqualizerGrid : MonoBehaviour
{
    public AudioSource mainTrack;

    public int xSize = 10;
    public int zSize = 5;

    public GameObject gridObject;

    public float durationToSample = 0.25f;
    public float punchIntensity = 25f;
    public float overallScale = 3f;

    public GameObject[] createdObjects;

    private bool _initComplete;
    private float _curTime;
    private Quaternion _initRotation;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (_initComplete)
            SampleTimer();
    }

    void SampleTimer()
    {
        _curTime += Time.deltaTime;

        if (_curTime > durationToSample)
        {
            ApplyAudioSourceFrequency();
            _curTime = 0f;
        }
    }

    void Init()
    {
        _initRotation = transform.rotation;
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        CreateGrid(xSize, zSize);

        transform.rotation = _initRotation;
        transform.localScale *= overallScale;

        _initComplete = true;
    }

    public void CreateGrid(int x, int z)
    {
        List<GameObject> createdObjs = new List<GameObject>();

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                var pos = new Vector3(i, 0f, j);
                var go = Instantiate(gridObject, pos + transform.position, Quaternion.identity) as GameObject;

                go.SetActive(true);
                go.transform.SetParent(transform);

                createdObjs.Add(go);
            }
        }

        createdObjects = createdObjs.ToArray();
    }

    private float[] _spectrum = new float[64];

    public void ApplyAudioSourceFrequency()
    {
        mainTrack.GetSpectrumData(_spectrum, 0, FFTWindow.Triangle);

        for (int i = 0; i < createdObjects.Length; i++)
        {
            var specVal = _spectrum[i];
            var go = createdObjects[i];

            go.transform.DOShakeScale(durationToSample, Vector3.up * specVal * punchIntensity);
        }
    }
}
