using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManyVSManyMode : GameModeBase
{
    List<int> randomRemove = new();

    public override void PlayGame()
    {
        Allys.ForEach(_ => _.Active());
        DOVirtual.DelayedCall(1, () =>
        {
            Enemys.ForEach(_ => _.Active());
        });
    }

    public override void SetUpGame(int level)
    {
        randomRemove.Clear();
        CreateCharacter("Player", GameManager.Ins.CharacterList[0], GameManager.Ins.PosAllys.GetTransform(2), GameConfig.Ins.PowerExtraDataPlayer, TeamType.Ally, true, new()); // Create player
        CreateCharacter("Ally 1", GameManager.Ins.CharacterList[1], GameManager.Ins.PosAllys.GetTransform(1), GameConfig.Ins.PowerExtraDataAlly, TeamType.Ally, false, GameConfig.Ins.useSkillAllys); // Create Ally
        CreateCharacter("Ally 2", GameManager.Ins.CharacterList[2], GameManager.Ins.PosAllys.GetTransform(3), GameConfig.Ins.PowerExtraDataAlly, TeamType.Ally, false, GameConfig.Ins.useSkillAllys); // Create Ally



        LevelGameData levelGameData = GameModeConfig.Ins.OneVsManyMode.LevelGameDatas[level - 1];

        int idxName = 0;
        foreach (var item in levelGameData.EnemyInfors)
        {
            for (int i = 0; i < item.Quanlity; i++)
            {
                idxName++;
                Transform posEnemy = GameManager.Ins.PosEnemys.GetTransform(GetPosRandom());
                CreateCharacter($"Enemy {idxName}", item.prefab, posEnemy, levelGameData.EnemyExtraData, TeamType.Enemy, false, levelGameData.UseSkills); // Create Enemy
            }
        }

    }

    private int GetPosRandom()
    {
        List<int> available = new List<int>();
        for (int i = 1; i <= 50; i++)
        {
            if (!randomRemove.Contains(i))
            {
                available.Add(i);
            }
        }

        if (available.Count == 0)
        {
            return -1;
        }

        int index = Random.Range(0, available.Count - 1);
        return available[index];
    }
}