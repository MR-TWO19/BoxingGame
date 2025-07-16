using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameModeData 
{
    public List<LevelGameData> LevelGameData;
}

[Serializable]
public class LevelGameData
{
    public List<CharacterState> UseSkills;
    public int DodgeRate;
    public EnemyInfor EnemyInfor;
    public CharacterData EnemyExtraData;
}

[Serializable]
public class EnemyInfor
{
    public int Quanlity;
    public GameObject prefab;
}


