using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class PhaseBase : MonoBehaviour
{
    protected abstract void Initialize();
    protected abstract void StartPhase();

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        StartPhase();
    }
}
