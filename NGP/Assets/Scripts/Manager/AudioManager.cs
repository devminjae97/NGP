using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSourcePool _audioSourcePool;
    
    // 재생중인 tag to audio dictionary
    private HashSet<AudioSourceHandle> _workingAudioSourceHandles = new();

    #region UniTask >>>>

    private CancellationTokenSource _mainCTS;

    #endregion UniTask <<<<
    
    public AudioSourceHandle PlaySoundOneShot(AudioClip audioClip)
        => PlaySound_Internal(audioClip, false);

    public AudioSourceHandle PlaySoundLoop(AudioClip audioClip)
        => PlaySound_Internal(audioClip, true);

    public void StopSound(AudioSourceHandle handle)
    {
        StopSound_Internal(handle);
    }

    public void StopAllSounds()
    {
        foreach (var handle in _workingAudioSourceHandles)
        {
            StopSound_Internal(handle);
        }
    }

    private AudioSourceHandle PlaySound_Internal(AudioClip audioClip, bool loop, float volume = 1.0f /*useless for now(24.05.02.) */)
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

        CheckMainCTS();
        
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.Play();
        
        CancellationTokenSource newCTS = CancellationTokenSource.CreateLinkedTokenSource(_mainCTS.Token);
        PrivateAudioSourceHandle handle = new(audioSource, audioClip, newCTS);
        
        _workingAudioSourceHandles.Add(handle);
        
        AsyncReleaseHandle(handle).Forget();

        return handle;
    }

    private void StopSound_Internal(AudioSourceHandle handle)
    {
        if (handle == null || _workingAudioSourceHandles.Contains(handle) == false)
        {
            Debug.Log($"[StopSound_Internal] something is missing. {handle == null} {_workingAudioSourceHandles.Contains(handle) == false}");
            return;
        }

        PrivateAudioSourceHandle privateHandle = (PrivateAudioSourceHandle)handle;
        var (audioSource, audioClip, cts) = privateHandle;

        if (audioSource == null || audioClip == null || cts == null)
        {
            Debug.Log($"[StopSound_Internal] element of handle is invalid.");
            return;
        }

        if (audioSource.enabled == false || audioSource.isPlaying == false)
        {
            //통과?
            //return;
        }

        audioSource.Stop();
    }

    private async UniTaskVoid AsyncReleaseHandle(PrivateAudioSourceHandle handle)
    {
        if (handle == null || _workingAudioSourceHandles.Contains(handle) == false)
        {  
            Debug.Log($"[AsyncReleaseOnFinished] something is missing. {handle == null} {_workingAudioSourceHandles.Contains(handle) == false}");
            return;
        }

        var (audioSource, audioClip, cts) = handle;
        
        if (audioSource == null || audioClip == null)
        {
            Debug.Log($"[AsyncReleaseOnFinished]{audioSource == null}, {audioClip == null}, {cts == null} ");
            return;
        }

        await UniTask.WaitUntil(() => audioSource.isPlaying == false, cancellationToken: handle.cts.Token);
        
        // Release에서 Stop 및 정리
        _audioSourcePool.Release(audioSource);
        _workingAudioSourceHandles.Remove(handle);
    }

    private void CheckMainCTS()
    {
        _mainCTS ??= CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
    }
    
    private void Awake()
    {
        // initialize pool
        BehaviourPool<AudioSource>.Initializer initializer = new (){owner = gameObject, defaultSize = 10, incrementalRate = 1.5f};
        _audioSourcePool = new(initializer);
        
        CheckMainCTS();
    }
    
    private class PrivateAudioSourceHandle : AudioSourceHandle
    {
        public CancellationTokenSource cts => _audioCTS;
        
        public PrivateAudioSourceHandle(AudioSource inAudioSource, AudioClip inAudioClip, CancellationTokenSource inAudioCTS) : base(inAudioSource, inAudioClip, inAudioCTS)
        {
            // Should be empty.
        }
        
        public void Deconstruct(out AudioSource outAudioSource, out AudioClip outAudioClip, out CancellationTokenSource outCTS)
        {
            outAudioSource = _audioSource;
            outAudioClip = _audioClip;
            outCTS = _audioCTS;
        }
    }

}


public class AudioSourceHandle
{
    protected AudioSource _audioSource;
    protected AudioClip _audioClip;
    protected CancellationTokenSource _audioCTS;
    
    protected AudioSourceHandle(AudioSource inAudioSource, AudioClip inAudioClip, CancellationTokenSource inAudioCTS)
    {
        _audioSource = inAudioSource;
        _audioClip = inAudioClip;
        _audioCTS = inAudioCTS;
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
            Debug.Log($"{toRelease.name} is null.");
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