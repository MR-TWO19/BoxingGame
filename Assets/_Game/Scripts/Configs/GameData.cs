using System;
using TwoCore;
using UnityEngine;

[Serializable]
public class UpdareData
{
    public InfoUpdare Speed;
    public InfoUpdare TimeCountdown;
}

[Serializable]
public class InfoUpdare
{
    public float value;
    public int Price;
}

[Serializable]
public class CarData : BaseData
{
    public GameObject Prefab;

    public CarInfoData Info;

    public bool IsDefaultUnlock;
    public bool IsUseStartGame;
    public int Price;
}

[Serializable]
public class CarInfoData
{
    public float BaseSpeed;
    public float BaseTimeCountdown;
}

[Serializable]
public class CarSkillSet
{
    public SkillData Accelerate;
    public SkillData Knockback;
    public SkillData Slow;
    public SkillData Power;
}

[Serializable]
public class CarSaveData
{
    public int CarID;

    public int Level;
    public bool IsUnlock;
    public bool IsUse;

    public float CurrSpeed;
    public float CurrTimeCountdown;

}


[Serializable]
public class SkillSaveData
{
    public int Level;
    public int Percent;
    public int Value;
    public float Time;
}

[Serializable]
public class SkillData
{
    public SkillType skillType;
    public Sprite Icon;
    public int Percent;
    public int Value;
    public float Time;
    public int UpPercent;
    public int UpValue;
    public float UpTime;
    public int Price;
}

public enum SkillType
{
    QuantitySlot,
    ExtraRoll,
    Immunity,
    Accelerate,
    Knockback,
    Slow,
    Power
}