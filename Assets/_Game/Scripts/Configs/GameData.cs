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
