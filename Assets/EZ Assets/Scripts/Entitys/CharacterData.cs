using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public float Head = 10;
    public float Speed = 1f;
    public float DamgeLeftHand = 1f;
    public float DamgeRightHand = 2f;

    public CharacterData(float head, float speed, float damgeLeftHand, float damgeRightHand)
    {
        Head = head;
        Speed = speed;
        DamgeLeftHand = damgeLeftHand;
        DamgeRightHand = damgeRightHand;
    }
}
