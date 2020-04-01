using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class AudioManager 
{
    AudioSource _audioInstance =null;
    public AudioManager()
    {
        if (_audioInstance == null)
            _audioInstance = new AudioSource();
    }

   void Play()
    {
        if (!_audioInstance.isPlaying)
            _audioInstance.Play();
    }

    void SetBGM(string str)
    {
       
        //_audioInstance.clip;
    }

}
