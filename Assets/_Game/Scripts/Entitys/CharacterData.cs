using System;
using TwoCore;
using UnityEngine;

[Serializable]
public class CharacterData : BaseData
{
    public GameObject Prefab;

    public bool IsUnlock;
    public bool IsUse;

    public float HP = 10;
    public float Speed = 0.1f;
    public float DamgeLeftHand = 1f;
    public float DamgeRightHand = 2f;
    public int Price;

    public CharacterData(string name, float hp, float speed, float damgeLeftHand, float damgeRightHand)
    {
        this.name = name;
        HP = hp;
        Speed = speed;
        DamgeLeftHand = damgeLeftHand;
        DamgeRightHand = damgeRightHand;
        Prefab = null;
    }

    public CharacterData(CharacterData other)
    {
        this.name = other.name;
        HP = other.HP;
        Speed = other.Speed;
        DamgeLeftHand = other.DamgeLeftHand;
        DamgeRightHand = other.DamgeRightHand;
        Prefab = other.Prefab;
        IsUnlock = other.IsUnlock;
        IsUse = other.IsUse;
    }
}
