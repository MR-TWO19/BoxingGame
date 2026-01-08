using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEditor;
using UnityEngine;

public class GameTab : TabContent
{
    private GameConfig gameConfig;

    public GameTab()
    {
        gameConfig = GameConfig.Ins;
    }

    public override void DoDraw()
    {

        EditorGUILayout.LabelField("Game Config", EditorStyles.boldLabel);

        //gameConfig.PriceUpgrade = EditorGUILayout.IntField("Price Upgrade", gameConfig.PriceUpgrade);
        //gameConfig.AddHP = EditorGUILayout.IntField("Add HP", gameConfig.AddHP);
        //gameConfig.AddATK = EditorGUILayout.IntField("Add ATK", gameConfig.AddATK);
    }
}
