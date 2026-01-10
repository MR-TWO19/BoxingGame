using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine;

public class PlayPopup : BasePopup
{
    [SerializeField] GameObject parents;
    [SerializeField] List<LevelButon> levelButons;
    [SerializeField] GameObject levelButtonPrefab;


    private static PlayPopup _instance;

    public static PlayPopup Show(GameMode gameMode)
    {
        if (_instance == null)
        {
            _instance = ShowWithParamsAndMethod<PlayPopup>("PlayPopup", PopupShowMethod.QUEUE, gameMode);
        }
        else
        {
            UIPopupManager.ShowPopup(_instance.UIPopup, true, false);
        }

        return _instance;
    }

    public static void HidePopup()
    {
        _instance.Hide();
    }

    protected override void SetParams(params object[] @params)
    {
        base.SetParams(@params);

        if (@params == null || @params.Length == 0)
            return;

        if (@params[0] is GameMode gameMode)
        {
            LoadLevel(gameMode);
        }
    }


    private void LoadLevel(GameMode gameMode)
    {
        levelButons.ForEach(_ => _.gameObject.SetActive(false));

        GameModeData gameModeData = gameMode switch
        {
            GameMode.OneVSOne => GameModeConfig.Ins.OneVsOneMode,
            GameMode.OneVSMany => GameModeConfig.Ins.OneVsManyMode,
            GameMode.ManeyVsMany => GameModeConfig.Ins.ManyVsManyMode,
            _ => null,
        };

        if (gameModeData == null) return;

        for (int i = 0; i < gameModeData.LevelGameDatas.Count; i++)
        {
            LevelButon levelButton;

            if (i < levelButons.Count)
            {
                levelButton = levelButons[i];
            }
            else
            {
                GameObject obj = Instantiate(levelButtonPrefab, parents.transform);
                levelButton = obj.GetComponent<LevelButon>();
                levelButons.Add(levelButton);
            }

            levelButton.gameObject.name = $"LevelButton{i}";
            levelButton.gameObject.SetActive(true);
            levelButton.SetUp(i + 1, gameMode);
        }
    }
}
