using System.Collections.Generic;
using TwoCore;
using UnityEngine;
using UnityEditor;

public class CarTab : TabContent
{
    private Vector2 scrollPos;
    private List<bool> carFoldouts = new List<bool>();

    //private List<CarData> carConfig => GameConfig.Ins.CarDatas;

    public override void DoDraw()
    {
        //if (carConfig == null)
        //{
        //    EditorGUILayout.HelpBox("Không tìm thấy CarDatas!", MessageType.Error);
        //    return;
        //}

        //Draw.Space(10);
        //Draw.Label("Car Config", EditorStyles.boldLabel);

        //// Nút thêm Car
        //if (Draw.Button("+ Add New Car", Color.cyan, Color.white, 200))
        //{
        //    var newCar = new CarData
        //    {
        //        id = carConfig.Count > 0 ? carConfig[^1].id + 1 : 0,
        //        name = $"Car {carConfig.Count + 1}",
        //        Info = new CarInfoData(),
        //    };
        //    carConfig.Add(newCar);
        //    carFoldouts.Add(true);
        //    Draw.SetDirty(GameConfig.Ins);
        //}

        //Draw.Space();
        //scrollPos = Draw.BeginScrollView(scrollPos);

        //for (int i = 0; i < carConfig.Count; i++)
        //{
        //    if (i >= carFoldouts.Count)
        //        carFoldouts.Add(false);

        //    var car = carConfig[i];

        //    Draw.BeginVertical("box", GUILayout.MaxWidth(900));
        //    Draw.BeginHorizontal();

        //    carFoldouts[i] = Draw.BeginFoldoutGroup(carFoldouts[i], $"Car {car.id} - {car.name}");

        //    if (Draw.Button("X", Color.red, Color.white, 40))
        //    {
        //        carConfig.RemoveAt(i);
        //        carFoldouts.RemoveAt(i);
        //        Draw.EndHorizontal();
        //        Draw.EndVertical();
        //        break;
        //    }

        //    Draw.EndFoldoutGroup();
        //    Draw.EndHorizontal();

        //    if (carFoldouts[i])
        //    {
        //        EditorGUI.indentLevel++;

        //        // ==============================
        //        // General
        //        // ==============================
        //        Draw.LabelBold("General");
        //        car.id = Draw.IntField("ID", car.id, 200);
        //        car.name = Draw.TextField("Name", car.name, 200);
        //        car.Prefab = Draw.ObjectField("Prefab", car.Prefab, 200);
        //        Draw.Space(15);

        //        // ==============================
        //        // Base Info
        //        // ==============================
        //        Draw.LabelBold("Base Info");
        //        car.Info.BaseSpeed = Draw.FloatField("Speed", car.Info.BaseSpeed, 200);
        //        car.Info.BaseTimeCountdown = Draw.FloatField("Countdown", car.Info.BaseTimeCountdown, 200);
        //        Draw.Space(15);

        //        // ==============================
        //        // Unlock & Price
        //        // ==============================

        //        Draw.Space(15);
        //        Draw.LabelBold("Unlock & Price $ Use");
        //        car.IsDefaultUnlock = Draw.ToggleField("Default Unlock", car.IsDefaultUnlock);
        //        car.IsUseStartGame = Draw.ToggleField("Use Start Game", car.IsUseStartGame);
        //        car.Price = Draw.IntField("Price", car.Price, 200);

        //        EditorGUI.indentLevel--;
        //    }

        //    Draw.EndVertical();
        //    Draw.Space();
        //}

        //Draw.EndScrollView();
    }
}
