using DG.Tweening;
using Doozy.Engine;
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

    private void Update()
    {
        if (GameMove != null)
        {
            GameMove.Allys.ForEach(_ => _.CustomUpdate());
            GameMove.Enemys.ForEach(_ => _.CustomUpdate());
        }    
    }

    public void LoadGame(int Level, GameMode gameMode)
    {
        GameModeEnum = gameMode;

        PlayPopup.HidePopup();

        GameEventMessage.SendEvent("GoToInGame", null);

        DOVirtual.DelayedCall(0.1f, () =>
        {
            switch (gameMode)
            {
                case GameMode.OneVSOne:
                    GameMove = new OnsVSOneMode();
                    break;
                case GameMode.OneVSMany:
                    GameMove = new OnsVSManyMode();
                    break;
                case GameMode.ManeyVsMany:
                    GameMove = new ManyVSManyMode();
                    break;
                default:
                    break;
            }

            GameMove.SetUpGame(Level);
            DOVirtual.DelayedCall(1, () =>
            {
                PlayGame();
            });
        });


    }

    public void PlayGame()
    {
        GameMove.PlayGame();
    }
}
