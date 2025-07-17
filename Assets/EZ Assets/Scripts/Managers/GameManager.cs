using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public PosCharacters PosEnemys;
    public PosCharacters PosAllys;
    public GameMode GameModeEnum;
    public List<GameObject> CharacterList;
    public IGameModeBase GameMove;

    public void SetUp(int Level, GameMode gameMode)
    {
        GameModeEnum = gameMode;
        switch (gameMode)
        {
            case GameMode.OneVSOne:
                GameMove = new OnsVSOneMode();
                break;
            case GameMode.OneVSMany:
                GameMove = new OnsVSOneMode();
                break;
            case GameMode.ManeyVsMany:
                break;
            default:
                break;  
        }

        GameMove.SetUpGame(Level);
        DOVirtual.DelayedCall(1, () =>
        {
            PlayGame();
            UIManager.Ins.ShowGamePlay();
        });
    }

    public void PlayGame()
    {
        GameMove.PlayGame();
    }
}
