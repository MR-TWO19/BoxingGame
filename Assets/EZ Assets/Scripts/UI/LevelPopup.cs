using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPopup : MonoBehaviour
{
    [SerializeField] GameObject parents;
    [SerializeField] List<LevelButon> levelButons;
    [SerializeField] GameObject levelButtonPrefab;
    
    public void Show(GameMode gameMode)
    {
        gameObject.SetActive(true);
        LoadLevel(gameMode);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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
