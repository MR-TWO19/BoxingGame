using DG.Tweening;
using Doozy.Engine;
using System.Collections;
using System.Collections.Generic;
using TwoCore;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public PosCharacters PosEnemys;
    public PosCharacters PosAllys;
    public GameMode GameModeEnum;
    public IGameModeBase GameMove;

    [SerializeField] private int coinWin;
    [SerializeField] private int coinLose;
    public int CoinWin { get => coinWin; set => coinWin = value; }
    public int CoinLose
    {
        get => GameModeEnum != GameMode.OneVSOne ? coinWin : coinLose;
        set => coinLose = value;
    }
    private int currLevel;
    private bool isLoadArena;

    private void Update()
    {
        if (GameMove != null)
        {
            GameMove.Allys.ForEach(_ => _.CustomUpdate());
            GameMove.Enemys.ForEach(_ => _.CustomUpdate());
        }    
    }

    public void LoadArena()
    {
        if (isLoadArena) return;

        Instantiate(GameConfig.Ins.ArenaPrefab, Vector3.zero, Quaternion.identity);

        isLoadArena = true;
    }

    public void SetUpGame(int Level, GameMode gameMode)
    {
        ResetGame();

        SoundManager.Ins.StopMusic();
        GameModeEnum = gameMode;
        currLevel = Level;

        PlayPopup.HidePopup();

        GameEventMessage.SendEvent("GoToInGame", null);

    }

    public void LoadGame()
    {
        switch (GameModeEnum)
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

        GameMove.SetUpGame(currLevel);
        DOVirtual.DelayedCall(1, () =>
        {
            SoundManager.Ins.PlayBGBattleMusic();
            SoundManager.Ins.PlayOneShot(SoundID.STARTGAME);
            PlayGame();
        });
    }

    public void PlayGame()
    {
        GameMove.PlayGame();
    }

    public void ResetGame()
    {
        if (GameMove == null) return;

        GameMove.ResetGame();
        GameMove = null;
    }

    public void NextGame() 
    {
        currLevel = GameModeEnum == GameMode.OneVSOne ? UserSaveData.Ins.Level : UserSaveData.Ins.LevelChallenge;

        ResetGame();

        LoadGame();
    }

    public void NextLevel()
    {
        if(GameModeEnum == GameMode.OneVSOne)   UserSaveData.Ins.NextLevel();
        else  UserSaveData.Ins.NextLevelChallenge();
    }
}
