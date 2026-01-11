using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using TwoCore;

public class OneVsManyTab : TabContent
{
    private GameModeConfig config;
    private Vector2 scrollPos;

    private List<bool> levelFoldouts = new();
    private List<bool> allyFoldouts = new();
    private List<bool> levelDataFoldouts = new();
    private List<bool> enemyFoldouts = new();

    public OneVsManyTab()
    {
        config = GameModeConfig.Ins;
    }

    public override void DoDraw()
    {
        if (config == null || config.OneVsManyMode == null)
        {
            EditorGUILayout.HelpBox("OneVsManyMode not found!", MessageType.Error);
            return;
        }

        var mode = config.OneVsManyMode;
        SyncFoldout(mode.LevelGameDatas.Count);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        //Draw.LabelBold("PLAYER EXTRA DATA");
        //DrawPowerExtra(mode.PlayerExtraData);

        Draw.Space(10);
        DrawLevelList(mode);

        EditorGUILayout.EndScrollView();
    }

    private void DrawLevelList(GameModeData mode)
    {
        for (int i = 0; i < mode.LevelGameDatas.Count; i++)
        {
            var level = mode.LevelGameDatas[i];

            levelFoldouts[i] =
                Draw.BeginFoldoutGroup(levelFoldouts[i], $"Level {i + 1}");

            if (levelFoldouts[i])
            {
                EditorGUI.indentLevel++;

                if (Draw.Button("X Level", Color.red, Color.white, 200))
                {
                    mode.LevelGameDatas.RemoveAt(i);
                    Draw.EndVertical();
                    break;
                }

                Draw.BeginVertical("box");

                //DrawAllyFoldout(mode, i);
                DrawLevelGameFoldout(level, i);

                Draw.EndVertical();
                EditorGUI.indentLevel--;
            }

            Draw.EndFoldoutGroup();
            Draw.Space(6);
        }

        if (Draw.Button("+ Add Level", Color.green, Color.white, 120))
            mode.LevelGameDatas.Add(new LevelGameData()
            {
                DodgeRate = 0,
                EnemyInfors = new(),
                UseSkills = new(),
            });
    }

    private void DrawAllyFoldout(GameModeData mode, int index)
    {
        allyFoldouts[index] =
            EditorGUILayout.Foldout(allyFoldouts[index], "ALLY", true);

        if (!allyFoldouts[index]) return;

        Draw.BeginVertical("box");
        DrawPowerExtra(mode.AllyExtraData);
        DrawSkillList("Ally Use Skills", mode.useSkillAllys);
        Draw.EndVertical();
    }

    private void DrawLevelGameFoldout(LevelGameData level, int index)
    {
        levelDataFoldouts[index] =
            EditorGUILayout.Foldout(levelDataFoldouts[index], "LEVEL GAME DATAS", true);

        if (!levelDataFoldouts[index]) return;

        Draw.BeginVertical("box");

        level.CoinWin = Draw.IntField("Coin", level.CoinWin, 200);
        level.DodgeRate = Draw.IntField("Dodge Rate", level.DodgeRate, 200);
        DrawSkillList("Use Skills", level.UseSkills);
        DrawEnemyFoldout(level, index);

        Draw.EndVertical();
    }

    private void DrawEnemyFoldout(LevelGameData level, int index)
    {
        enemyFoldouts[index] =
            EditorGUILayout.Foldout(enemyFoldouts[index], "ENEMIES", true);

        if (!enemyFoldouts[index]) return;

        Draw.BeginVertical("box");
        DrawEnemyList(level.EnemyInfors);
        Draw.EndVertical();
    }

    private void SyncFoldout(int count)
    {
        while (levelFoldouts.Count < count) levelFoldouts.Add(false);
        while (allyFoldouts.Count < count) allyFoldouts.Add(false);
        while (levelDataFoldouts.Count < count) levelDataFoldouts.Add(false);
        while (enemyFoldouts.Count < count) enemyFoldouts.Add(false);
    }

    #region COMMON
    private void DrawPowerExtra(PowerExtraData data)
    {
        if (data == null) return;

        Draw.BeginVertical("box");
        data.HP = Draw.FloatField("HP", data.HP, 200);
        data.Speed = Draw.FloatField("Speed", data.Speed, 200);
        data.DamgeLeftHand = Draw.FloatField("Damage Left", data.DamgeLeftHand, 200);
        data.DamgeRightHand = Draw.FloatField("Damage Right", data.DamgeRightHand, 200);
        Draw.EndVertical();
    }

    private void DrawSkillList(string label, List<CharacterState> list)
    {
        if (list == null) return;

        Draw.LabelBold(label);

        for (int i = 0; i < list.Count; i++)
        {
            Draw.BeginHorizontal("box");
            list[i] = (CharacterState)EditorGUILayout.EnumPopup(list[i]);

            if (Draw.Button("X", Color.red, Color.white, 30))
            {
                list.RemoveAt(i);
                break;
            }
            Draw.EndHorizontal();
        }

        if (Draw.Button("+ Add Skill", Color.green, Color.white, 120))
            list.Add(default);
    }

    private void DrawEnemyList(List<EnemyInfor> enemies)
    {
        if (enemies == null) return;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];

            Draw.BeginVertical("box");
            enemy.Quanlity = Draw.IntField("Quantity", enemy.Quanlity, 200);
            enemy.CharacterID = Draw.IntPopupField("Character ID", enemy.CharacterID, GameConfig.Ins.CharaterDatas, "name", "id", 200);
            DrawPowerExtra(enemy.EnemyExtraData);

            if (Draw.Button("Remove Enemy", Color.red, Color.white, 120))
            {
                enemies.RemoveAt(i);
                break;
            }
            Draw.EndVertical();
        }

        if (Draw.Button("+ Add Enemy", Color.green, Color.white, 120))
            enemies.Add(new EnemyInfor());
    }
    #endregion
}
