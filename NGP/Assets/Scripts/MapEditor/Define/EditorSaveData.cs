using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CrackedBlockInfo
{
    public float respawnTime;
    public Vector2Int pos;
    public CrackedBlockInfo( float _respawnTime, Vector2Int _pos )
    {
        respawnTime = _respawnTime; 
        pos = _pos;
    }
}

namespace SaveDataContainer
{
    [System.Serializable]
    public struct NormalBlockData
    {
        public List<Vector2Int> poses;
    }
    [System.Serializable]
    public struct FlagData
    {
        public Vector2Int startFlagPos;
        public Vector2Int goalFlagPos;
    }

    [System.Serializable]
    public struct CrackedBlockData
    {
        public List<CrackedBlockInfo> infos;
    }
}
