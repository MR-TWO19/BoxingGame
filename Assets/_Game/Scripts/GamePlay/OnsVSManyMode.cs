using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnsVSManyMode : GameModeBase
{
    List<int> randomRemove = new();

    private int coin;

    public override void PlayGame()
    {
        Allys.ForEach(_ => _.Active());
        DOVirtual.DelayedCall(0.2f, () =>
        {
            Enemys.ForEach(_ => _.Active());
        });
    }

    public override void SetUpGame(int level)
    {
        randomRemove.Clear();
        var data = UserSaveData.Ins.GetCharacterUse();

        GameObject playerPrefab = GameConfig.Ins.GetCharacterData(data.CharacterID).Prefab;
        Transform posPlayer = GameManager.Ins.PosAllys.GetTransform(2);
        PowerExtraData powerExtraData = new()
        {
            HP = UserSaveData.Ins.CharacterSaveData.CurrHP,
            Speed = UserSaveData.Ins.CharacterSaveData.CurrSpeed,
            DamgeLeftHand = UserSaveData.Ins.CharacterSaveData.DamageLeft,
            DamgeRightHand = UserSaveData.Ins.CharacterSaveData.DamageRight,
        };

        CreateCharacter("Player", playerPrefab, posPlayer, powerExtraData, TeamType.Ally, true, new()); // Create player

        LevelGameData levelGameData = GameModeConfig.Ins.OneVsManyMode.LevelGameDatas[level - 1];

        GameManager.Ins.CoinWin = 0;
        coin = levelGameData.CoinWin;

        int idxName = 0;
        foreach (var item in levelGameData.EnemyInfors)
        {
            for (int i = 0; i < item.Quanlity; i++)
            {
                idxName++;
                Transform posEnemy = GameManager.Ins.PosEnemys.GetTransform(GetPosRandom());
                GameObject enemyPrefab = GameConfig.Ins.GetCharacterData(item.CharacterID).Prefab;
                CreateCharacter($"Enemy {idxName}", enemyPrefab, posEnemy, item.EnemyExtraData, TeamType.Enemy, false, levelGameData.UseSkills); // Create Enemy
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

    public override void EnemyDead(Character character)
    {
        base.EnemyDead(character);

        GameManager.Ins.CoinWin += coin;
    }
}