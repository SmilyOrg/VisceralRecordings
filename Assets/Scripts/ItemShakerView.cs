using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemShakerView : MonoBehaviour
{
    public AudioSource audio;
    public GameObject[] objs;
    public ShakeStyle style;

    public float timerSampleThreshold = 0.25f;
    public float punchIntensity = 1f;
    public bool combineAllModes = false;

    private float _currentTime;

    public enum ShakeStyle
    {
        Position,
        Rotation,
        Scale
    }

    private void Update()
    {
        Timer();
    }

    void Timer()
    {
        _currentTime += Time.deltaTime;

        if(_currentTime >= timerSampleThreshold)
        {
            _currentTime = 0f;
            ApplyAudioSourceFrequency();
        }
    }

    private float[] _spectrum = new float[64];

    public void ApplyAudioSourceFrequency()
    {
        audio.GetSpectrumData(_spectrum, 0, FFTWindow.Triangle);

        for (int i = 0; i < objs.Length; i++)
        {
            var specVal = _spectrum[i];
            var go = objs[i];

            Sequence move = DOTween.Sequence();
            Sequence rotate = DOTween.Sequence();
            Sequence scale = DOTween.Sequence();

            move.Append(go.transform.DOShakePosition(timerSampleThreshold, Vector3.one * specVal * punchIntensity));
            rotate.Append(go.transform.DOShakeRotation(timerSampleThreshold, Vector3.right * specVal * punchIntensity));
            scale.Append(go.transform.DOShakeScale(timerSampleThreshold, Vector3.one * specVal * punchIntensity));

            if(!combineAllModes)
            {
                switch (style)
                {
                    case ShakeStyle.Position:
                        move.Play();
                        break;
                    case ShakeStyle.Rotation:
                        rotate.Play();
                        break;
                    case ShakeStyle.Scale:
                       scale.Play();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Sequence fullSeq = DOTween.Sequence();
                fullSeq.Insert(0, move);
                fullSeq.Insert(0, rotate);
                fullSeq.Insert(0, scale);

                fullSeq.Play();
            }
        }
    }
}
