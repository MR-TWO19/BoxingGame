using System.Collections.Generic;
using TwoCore;
using UnityEngine;
using UnityEditor;

public class CharacterTab : TabContent
{
    private Vector2 scrollPos;
    private List<bool> charFoldouts = new List<bool>();

    private List<CharacterData> charConfig => GameConfig.Ins?.CharaterDatas;

    public override void DoDraw()
    {
        if (charConfig == null)
        {
            EditorGUILayout.HelpBox("Không tìm thấy CharaterDatas!", MessageType.Error);
            return;
        }

        Draw.Space(10);
        Draw.Label("Character Config", EditorStyles.boldLabel);

        // Add new Character button
        if (Draw.Button("+ Add New Character", Color.cyan, Color.white, 200))
        {
            var newChar = new CharacterData($"Character {charConfig.Count + 1}", 10f, 0.1f, 1f, 2f);
            // try set id if BaseData provides it
            try
            {
                newChar.id = charConfig.Count > 0 ? charConfig[^1].id + 1 : 0;
            }
            catch { }

            charConfig.Add(newChar);
            charFoldouts.Add(true);
            Draw.SetDirty(GameConfig.Ins);
        }

        Draw.Space();
        scrollPos = Draw.BeginScrollView(scrollPos);

        for (int i = 0; i < charConfig.Count; i++)
        {
            if( i >= charFoldouts.Count)
                charFoldouts.Add(false);

            var ch = charConfig[i];

            Draw.BeginVertical("box", GUILayout.MaxWidth(900));
            Draw.BeginHorizontal();

            charFoldouts[i] = Draw.BeginFoldoutGroup(charFoldouts[i], $"Char {GetIdSafe(ch)} - {ch.name}");

            if (Draw.Button("X", Color.red, Color.white, 40))
            {
                charConfig.RemoveAt(i);
                charFoldouts.RemoveAt(i);
                Draw.EndHorizontal();
                Draw.EndVertical();
                Draw.SetDirty(GameConfig.Ins);
                break;
            }

            Draw.EndFoldoutGroup();
            Draw.EndHorizontal();

            if (charFoldouts[i])
            {
                EditorGUI.indentLevel++;

                Draw.LabelBold("General");
                // ID field (if available)
                try
                {
                    ch.id = Draw.IntField("ID", ch.id, 200);
                }
                catch { }

                ch.name = Draw.TextField("Name", ch.name, 200);
                ch.Prefab = Draw.ObjectField("Prefab", ch.Prefab, 200);

                Draw.Space(6);
                ch.IsUnlock = Draw.ToggleField("Default Unlock", ch.IsUnlock);
                ch.IsUse = Draw.ToggleField("Use Start Game", ch.IsUse);

                ch.HP = Draw.FloatField("HP", ch.HP, 200);
                ch.Speed = Draw.FloatField("Speed", ch.Speed, 200);
                ch.DamgeLeftHand = Draw.FloatField("Damage Left", ch.DamgeLeftHand, 200);
                ch.DamgeRightHand = Draw.FloatField("Damage Right", ch.DamgeRightHand, 200);
                ch.Price = Draw.IntField("Price", ch.Price, 200);

                EditorGUI.indentLevel--;
                Draw.SetDirty(GameConfig.Ins);
            }

            Draw.EndVertical();
            Draw.Space();
        }

        Draw.EndScrollView();
    }

    private int GetIdSafe(CharacterData ch)
    {
        try { return ch.id; } catch { return -1; }
    }
}
