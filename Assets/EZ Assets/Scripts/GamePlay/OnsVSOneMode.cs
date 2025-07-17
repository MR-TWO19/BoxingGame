using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnsVSOneMode : GameModeBase
{
    public override void PlayGame()
    {
        Allys.ForEach(_ => _.Active());
        DOVirtual.DelayedCall(3, () =>
        {
            Enemys.ForEach(_ => _.Active());
        });
    }

    public override void SetUpGame(int level)
    {
        GameObject playerPrefab = GameManager.Ins.CharacterList[0];
        Transform posPlayer = GameManager.Ins.PosAllys.GetTransform(4);
        CreateCharacter("Player", playerPrefab, posPlayer, GameConfig.Ins.PowerExtraDataPlayer, TeamType.Ally, true, new()); // Create player

        LevelGameData levelGameData = GameModeConfig.Ins.OneVsOneMode.LevelGameDatas[level - 1];
        for (int i = 0; i < levelGameData.EnemyInfors[0].Quanlity; i++)
        {
            Transform posEnemy = GameManager.Ins.PosEnemys.GetTransform(32);
            CreateCharacter($"Enemy {i+1}", levelGameData.EnemyInfors[0].prefab, posEnemy, levelGameData.EnemyExtraData, TeamType.Enemy, false, levelGameData.UseSkills); // Create Enemy
        }
    }
}
