using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private SoundPack _soundPack;
    
    private AudioSourcePool _audioSourcePool;
    

    // 재생중인 tag to audio dictionary
    //private Dictionary<string, AudioSource> _workingAudioSources = new();
    private HashSet<AudioSourceHandle> _workingAudioSourceHandles = new();
    //private HashSet<IEnumerator> _pendingReleaseIEs = new();

    public void PlaySoundOneShot( /* audio tag */)
    {
        
    }

    public AudioSourceHandle PlaySoundOneShot(AudioClip audioClip)
        => PlaySound_Internal(audioClip, false);

    public void PlaySoundLoop( /* audio tag */)
    {
        
    }

    public AudioSourceHandle PlaySoundLoop(AudioClip audioClip)
        => PlaySound_Internal(audioClip, true);

    public void StopSound( /* audio tag */)
    {
        
    }

    public void StopSound(AudioSourceHandle handle)
    {
        StopSound_Internal(handle);
    }

    /*
    public void StopSound(AudioClip audioClip)
    {
        StopSound_Internal(audioClip);
    }
    */

    public void StopAllSounds()
    {
        foreach (var handle in _workingAudioSourceHandles)
        {
            StopSound_Internal(handle);
        }
    }

    // @TODO, 같은 클립 빠르게 재생하면 release가 제대로 안 됨
    //Coroutine문제 -> Task/ UniTask로 바꿔야함
    private AudioSourceHandle PlaySound_Internal(AudioClip audioClip, bool loop, float volume = 1.0f)
    {
        if (audioClip == null)
        {
            return null;
        }
        
        if (_audioSourcePool.IsValid() == false)
        {
            return null;
        }
        
        var audioSource = _audioSourcePool.Get();
        
        // 방어코드
        if (audioSource == null || audioSource.isPlaying)
        {
            return null;
        }

        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.Play();
        
        AudioSourceHandle handle = new() { audioSource = audioSource, audioClip = audioClip, releaseIE = null };
        var releaseIE = IEReleaseOnFinished(handle);
        //handle.releaseIE = releaseIE;
        
        if (loop == false)
        {
            Debug.Log($"[PlaySound_Internal] loop StartCoroutine() AS: {audioSource.GetHashCode()}");
            StartCoroutine(releaseIE);
            //_pendingReleaseIEs.Add(releaseIE);
        }
        
        Debug.Log($"[PlaySound_Internal] non loop StartCoroutine() AS: {audioSource.GetHashCode()}");
        _workingAudioSourceHandles.Add(handle);

        return handle;
    }
    
    /**
     *  같은 AudioSource를 공유하며 다른 클립을 플레이 하면
     *  
     */
    private void StopSound_Internal(AudioSourceHandle handle)
    {
        if (handle == null || _workingAudioSourceHandles.Contains(handle) == false)
        {
            return;
        }
        
        var (audioSource, audioClip, releaseIE) = handle;
        
        if (audioSource == null || audioClip == null || releaseIE == null)
        {
            return;
        }

        if (audioSource.enabled == false)
        {
            return;
        }

        StopCoroutine(releaseIE);
        _workingAudioSourceHandles.Remove(handle);
        _audioSourcePool.Release(audioSource);
    }
    
    // @TODO, Task로 바꿔서 CancellationToken: Stop/StopAll에 요긴하게 쓰일듯
    private IEnumerator IEReleaseOnFinished(AudioSourceHandle handle)
    {
        // MonobehaviourHandler를 만들어서 거기서 돌려?
        
        //Debug.Log($"[IEReleaseOnFinished] Start.");
        if (handle == null || _workingAudioSourceHandles.Contains(handle) == false)
        {  
            Debug.Log($"[IEReleaseOnFinished] something is missing. {handle == null} {_workingAudioSourceHandles.Contains(handle) == false}");
            yield break;
        }

        var (audioSource, audioClip, releaseIE) = handle;
        
        if (audioSource == null || audioClip == null /*|| releaseIE == null*/)
        {
            Debug.Log($"[IEReleaseOnFinished]{audioSource == null}, {audioClip == null}, {releaseIE == null} ");
            yield break;
        }

        //Debug.Log($"[IEReleaseOnFinished] Wait for {audioSource.GetHashCode()}");
        yield return new WaitUntil(() => audioSource.isPlaying == false);

        //Debug.Log($"[IEReleaseOnFinished] {audioSource.GetHashCode()} finished.");
        StopSound_Internal(handle);
    }
    
    private void Awake()
    {
        // initialize pool
        BehaviourPool<AudioSource>.Initializer initializer = new (){owner = gameObject, defaultSize = 10, incrementalRate = 1.5f};
        _audioSourcePool = new(initializer);
    }
}

public class AudioSourceHandle
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public IEnumerator releaseIE;

    public void Deconstruct(out AudioSource outAudioSource, out AudioClip outAudioClip, out IEnumerator outReleaseIE)
    {
        outAudioSource = audioSource;
        outAudioClip = audioClip;
        outReleaseIE = releaseIE;
    }
}

public class AudioSourcePool : BehaviourPool<AudioSource>
{
    public AudioSourcePool(Initializer initializer) : base(initializer)
    {
    }

    public override void Release(AudioSource toRelease)
    {
        if (toRelease == null)
        {
            return;
        }
        
        //Log
        if (toRelease.enabled == false)
        {
            Debug.Log($"{toRelease.name} is already disabled.");
        }
        // <----
        toRelease.Stop();
        toRelease.loop = false;
        toRelease.clip = null;
        base.Release(toRelease);
    }
}