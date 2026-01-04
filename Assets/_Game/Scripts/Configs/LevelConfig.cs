using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/LevelConfig")]
public class LevelConfig : ConfigBase<LevelConfig>
{
    public List<LevelData> levelDatas;
}

[Serializable]
public class LevelData
{
    public List<BotData> BotDatas;
    public int TotalLap;
    public int GoldWin;
    public int GoldLose;
}

[Serializable]
public class BotData
{
    public int CarId;
    public float Speed;
    public float TimeUseSkill;
    public float RateNote;
    public float RateThreeOfAKind;
    public float RateSmallStraight;
    public float RateFullHouse;
    public float RateFourOfAKind;
    public float RateYatzy;

}
