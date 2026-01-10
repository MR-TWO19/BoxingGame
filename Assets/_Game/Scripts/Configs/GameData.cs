using System;
using UnityEngine;


[Serializable]
public class UpdareData
{
    public InfoUpdare HP;
    public InfoUpdare Speed;
    public InfoUpdare LATK;
    public InfoUpdare RATK;
}

[Serializable]
public class InfoUpdare
{
    public float value;
    public int Price;
}

[Serializable]
public class CharacterSaveData
{
    public int CharacterID;
    public int Level = 1;
    public bool IsUnlock;
    public bool IsUse;

    public float CurrHP = 10;
    public float CurrSpeed = 0.1f;
    public float DamageLeft = 1f;
    public float DamageRight = 2f;
}