using System.Collections.Generic;
using TwoCore;
using UnityEditor;
using UnityEngine;

public class LevelTab : TabContent
{
    private LevelConfig levelConfig;
    private Vector2 scrollPos;

    // Foldout cho level
    private List<bool> levelFoldouts = new List<bool>();

    public LevelTab()
    {
        levelConfig = LevelConfig.Ins;
    }

    public override void DoDraw()
    {
        //if (levelConfig == null)
        //{
        //    EditorGUILayout.HelpBox("Không tìm thấy LevelConfig!", MessageType.Error);
        //    return;
        //}

        //Draw.Space(10);
        //Draw.Label("Level Config", EditorStyles.boldLabel);

        //// Nút thêm Level
        //if (Draw.Button("Thêm Level", Color.red, Color.white, 120))
        //{
        //    var newLevel = new LevelData
        //    {
        //        BotDatas = new List<BotData>()
        //    };
        //    levelConfig.levelDatas.Add(newLevel);
        //    levelFoldouts.Add(true);
        //}

        //Draw.Space();
        //scrollPos = Draw.BeginScrollView(scrollPos);

        //for (int i = 0; i < levelConfig.levelDatas.Count; i++)
        //{
        //    if (i >= levelFoldouts.Count)
        //        levelFoldouts.Add(false);

        //    var level = levelConfig.levelDatas[i];

        //    Draw.BeginVertical("box", GUILayout.MaxWidth(900));
        //    Draw.BeginHorizontal();

        //    levelFoldouts[i] = Draw.BeginFoldoutGroup(levelFoldouts[i], $"Level {i + 1}");

        //    if (Draw.Button("X", Color.red, Color.white, 40))
        //    {
        //        levelConfig.levelDatas.RemoveAt(i);
        //        levelFoldouts.RemoveAt(i);
        //        Draw.EndHorizontal();
        //        Draw.EndVertical();
        //        continue; // tránh lỗi khi remove
        //    }

        //    Draw.EndFoldoutGroup();
        //    Draw.EndHorizontal();

        //    if (levelFoldouts[i])
        //    {
        //        EditorGUI.indentLevel++;

        //        level.TotalLap = Draw.IntField("Total Lap", level.TotalLap, 200);
        //        level.GoldWin = Draw.IntField("Gold Win", level.GoldWin, 200);
        //        level.GoldLose = Draw.IntField("Gold Win", level.GoldLose, 200);
        //        Draw.LabelBold("Bot Datas");
        //        Draw.Space();

        //        for (int b = 0; b < level.BotDatas.Count; b++)
        //        {
        //            var bot = level.BotDatas[b];

        //            Draw.BeginVertical("box");
        //            Draw.BeginHorizontal();
        //            Draw.Label($"Bot {b + 1}", EditorStyles.boldLabel);

        //            if (Draw.Button("X", Color.red, Color.white, 40))
        //            {
        //                level.BotDatas.RemoveAt(b);
        //                Draw.EndHorizontal();
        //                Draw.EndVertical();
        //                continue;
        //            }

        //            Draw.EndHorizontal();

        //            bot.CarId = Draw.IntPopupField("Car", bot.CarId, GameConfig.Ins.CarDatas, "name", "id", 200);
        //            bot.Speed = Draw.FloatField("Speed", bot.Speed, 200);
        //            bot.TimeUseSkill = Draw.FloatField("TimeUseSkill", bot.TimeUseSkill, 200);
        //            bot.RateThreeOfAKind = Draw.FloatField("RateThreeOfAKind", bot.RateThreeOfAKind, 200);
        //            bot.RateSmallStraight = Draw.FloatField("RateSmallStraight", bot.RateSmallStraight, 200);
        //            bot.RateFullHouse = Draw.FloatField("RateFullHouse", bot.RateFullHouse, 200);
        //            bot.RateFourOfAKind = Draw.FloatField("RateFourOfAKind", bot.RateFourOfAKind, 200);
        //            bot.RateYatzy = Draw.FloatField("RateYatzy", bot.RateYatzy, 200);

        //            Draw.EndVertical();
        //            Draw.Space(5);
        //        }

        //        if (Draw.Button("+ Add Bot", Color.green, Color.white, 120))
        //        {
        //            level.BotDatas.Add(new BotData());
        //        }

        //        EditorGUI.indentLevel--;
        //    }

        //    Draw.EndVertical();
        //    Draw.Space();
        //}

        //Draw.EndScrollView();
    }
}
