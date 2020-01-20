using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{
    public List<LevelData> levelsMap = new List<LevelData>();
}

[SerializeField]
public class LevelData
{
    public int level;
    public bool isUnLocked;
    public bool isCompleted;
}
