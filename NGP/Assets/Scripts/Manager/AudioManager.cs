using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    // Addressable을 이용한 Audio 재생
    
    // List<AudioSource> AudioSourcePool

    // 재생중인 tag to audio dictionary
    // Dictionary<Tag, AudioSource> WorkingAudioSource;
    
    // @TODO, audioSource pooling 개수? -> 늘어나게?

    void PlaySoundOneShot( /* audio tag */)
    {
        
    }
    
    void PlaySoundLoop( /* audio tag */)
    {
        
    }

    void StopSound( /* audio tag */)
    {
        
    }
    
    void StopAllSounds()
    {
        
    }
}
