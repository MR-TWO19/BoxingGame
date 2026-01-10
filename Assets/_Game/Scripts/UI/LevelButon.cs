using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButon : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] private GameObject objLock;

    int level;
    GameMode gameMode;

    private void Start()
    {
        btn.onClick.AddListener(PlayGame);
    }

    private void PlayGame()
    {
        GameManager.Ins.LoadGame(level, gameMode);
    }

    public void SetUp(int level, GameMode gameMode)
    {
        txtLevel.text = level.ToString();
        this.gameMode = gameMode;
        this.level = level;
        gameObject.SetActive(true);

        bool isLock = level > UserSaveData.Ins.Level;

        objLock.SetActive(isLock);
        btn.interactable = !isLock;
    }


}
