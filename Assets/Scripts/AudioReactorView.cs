using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactorView : MonoBehaviour {

    public AudioSource audio;
    public ResonanceAudioSource resAudio;
    public ParticleSystem[] particles;

    private bool _particlesTriggerd = false;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		if(CheckAudioPlay())
        {
            PlayParticles();
        }
        else
        {
            PlayParticles(false);
        }

	}

    public void PlayParticles(bool active = true)
    {
        //ParticleSystem.EmissionModule emission;

        foreach (var particle in particles)
        {
            //emission = particle.emission;

            if (active)
            {
                //emission.enabled = true;
                particle.Play();
            }
            else
            {
                particle.Stop();
                //emission.enabled = false;
            }
        }
    }

    bool CheckAudioPlay()
    {
        if(audio.isPlaying)
        {
            return true;
        }
        return false;
    }

    void Init()
    {
        GatherComponents();
    }

    void GatherComponents()
    {
        if (!audio)
            audio = GetComponent<AudioSource>();
        if (!audio)
            Debug.Log("Audiosource missing on: " + gameObject.name);

        if (!resAudio)
            resAudio = GetComponent<ResonanceAudioSource>();
        if(!resAudio)
            Debug.Log("Audiosource missing on: " + gameObject.name);
    }
}
