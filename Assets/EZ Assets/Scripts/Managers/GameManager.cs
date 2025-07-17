using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public PosCharacters PosEnemys;
    public PosCharacters PosAllys;

    public List<GameObject> CharacterList;

    public IGameModeBase GameMove;

    private void Start()
    {
        SetUp(GameMode.OneVSOne);
        DOVirtual.DelayedCall(3, () =>
        {
            PlayGame();
        });
    }

    public void SetUp(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.OneVSOne:
                GameMove = new OnsVSOneMode();
                break;
            case GameMode.OneVSMany:
                break;
            case GameMode.ManeyVsMany:
                break;
            default:
                break;  
        }

        GameMove.SetUpGame(1);
    }

    public void PlayGame()
    {
        GameMove.PlayGame();
    }

}
