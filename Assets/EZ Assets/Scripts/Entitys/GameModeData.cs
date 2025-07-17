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
    public List<EnemyInfor> EnemyInfors;
    public PowerExtraData EnemyExtraData;
}

[Serializable]
public class EnemyInfor
{
    public int Quanlity;
    public GameObject prefab;
}

[Serializable]
public class PowerExtraData
{
    public float HP;
    public float Speed;
    public float DamgeLeftHand;
    public float DamgeRightHand;
}

