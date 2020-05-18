using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    [SerializeField]
    AudioSource _eventAudioSource;
    [SerializeField]
    AudioClip _buttonClick, _lifeforcegain;


    int test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ButtonClick()
    {
        _eventAudioSource.clip = _buttonClick;
        _eventAudioSource.Play();
    }

    public void LifeForceGained()
    {
        _eventAudioSource.clip = _lifeforcegain;
        _eventAudioSource.Play();
    }
}
