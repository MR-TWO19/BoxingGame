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
        GameManager.Ins.SetUpGame(level, gameMode);
    }

    public void SetUp(int level, GameMode gameMode)
    {
        txtLevel.text = level.ToString();
        this.gameMode = gameMode;
        this.level = level;
        gameObject.SetActive(true);

        int currLevel = gameMode switch
        {
            GameMode.OneVSOne => UserSaveData.Ins.Level,
            GameMode.OneVSMany => UserSaveData.Ins.LevelChallenge,
            GameMode.ManeyVsMany => UserSaveData.Ins.Level,
            _ => 0
        };

        bool isLock = level > currLevel;

        objLock.SetActive(isLock);
        btn.interactable = !isLock;
    }


}
