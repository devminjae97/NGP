using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMapData
{
    // private 이라면 save가 되지 않음
    public SaveDataContainer.FlagData[] flag;
    public SaveDataContainer.NormalBlockData[] normalBlock;
    public SaveDataContainer.CrackedBlockData[] crackedBlock;

    public EditorMapData()
    {
        flag = new SaveDataContainer.FlagData[2];
        flag[0].startFlagPos = new Vector2Int();
        flag[1].startFlagPos = new Vector2Int();
        flag[0].goalFlagPos = new Vector2Int();
        flag[1].goalFlagPos = new Vector2Int();
        normalBlock = new SaveDataContainer.NormalBlockData[2];
        normalBlock[0].poses = new List<Vector2Int>();
        normalBlock[1].poses = new List<Vector2Int>();
        crackedBlock = new SaveDataContainer.CrackedBlockData[2];
        crackedBlock[0].infos = new List<CrackedBlockInfo>();
        crackedBlock[1].infos = new List<CrackedBlockInfo>();
    }
}
