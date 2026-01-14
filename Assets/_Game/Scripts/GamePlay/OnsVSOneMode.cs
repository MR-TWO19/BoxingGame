using DG.Tweening;
using UnityEngine;

public class OnsVSOneMode : GameModeBase
{
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
        var data = UserSaveData.Ins.GetCharacterUse();

        GameObject playerPrefab = GameConfig.Ins.GetCharacterData(data.CharacterID).Prefab;
        Transform posPlayer = GameManager.Ins.PosAllys.GetTransform(4);

        PowerExtraData powerExtraData = new()
        {
            HP = UserSaveData.Ins.CharacterSaveData.CurrHP,
            Speed = UserSaveData.Ins.CharacterSaveData.CurrSpeed,
            DamgeLeftHand = UserSaveData.Ins.CharacterSaveData.DamageLeft,
            DamgeRightHand = UserSaveData.Ins.CharacterSaveData.DamageRight,
        };

        CreateCharacter("Player", playerPrefab, posPlayer, powerExtraData, TeamType.Ally, true, new()); // Create player

        LevelGameData levelGameData = GameModeConfig.Ins.OneVsOneMode.LevelGameDatas[level - 1];

        GameManager.Ins.CoinWin = levelGameData.CoinWin;
        GameManager.Ins.CoinLose = levelGameData.CoinLose;

        for (int i = 0; i < levelGameData.EnemyInfors[0].Quanlity; i++)
        {
            Transform posEnemy = GameManager.Ins.PosEnemys.GetTransform(32);

            GameObject enemyPrefab = GameConfig.Ins.GetCharacterData(levelGameData.EnemyInfors[i].CharacterID).Prefab;
            CreateCharacter($"Enemy {i+1}", enemyPrefab, posEnemy, levelGameData.EnemyInfors[0].EnemyExtraData, TeamType.Enemy, false, levelGameData.UseSkills); // Create Enemy
        }
    }
}
