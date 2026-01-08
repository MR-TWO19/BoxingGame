using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameModeData 
{
    public PowerExtraData PlayerExtraData;
    public PowerExtraData AllyExtraData;
    public List<CharacterState> useSkillAllys;
    public List<LevelGameData> LevelGameDatas;
}

[Serializable]
public class LevelGameData
{
    public List<CharacterState> UseSkills;
    public int DodgeRate;
    public List<EnemyInfor> EnemyInfors;
}

[Serializable]
public class EnemyInfor
{
    public int Quanlity;
    public int CharacterID;
    public PowerExtraData EnemyExtraData;
}

[Serializable]
public class PowerExtraData
{
    public float HP;
    public float Speed;
    public float DamgeLeftHand;
    public float DamgeRightHand;
}

