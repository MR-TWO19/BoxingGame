using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public void PlayGame(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.OneVSOne:
                PlayModeOneVSOne();
                break;
            case GameMode.OneVSMany:
                break;
            case GameMode.ManeyVsMany:
                break;
            default:
                break;  
        }
    }

    private void PlayModeOneVSOne()
    {

    }
}
