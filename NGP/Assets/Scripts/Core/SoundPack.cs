using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Data", menuName = "Scriptable Object/Sound Data", order = int.MaxValue)]
public class SoundPack : ScriptableObject
{
    [SerializeField] private List<SoundObject> _soundObjects;
    public List<SoundObject> soundObjects => _soundObjects;
}

[Serializable]
public struct SoundObject : ISerializationCallbackReceiver
{
    [SerializeField] private bool _useFileNameAsVar;
    [SerializeField] private string _name;
    [SerializeField] private AudioClip _audio;
    
    public void OnBeforeSerialize()
    {
        if (_audio != null && _useFileNameAsVar)
        { 
            _name = _audio.name;
        }
    }

    public void OnAfterDeserialize()
    {
        return;
    }
}