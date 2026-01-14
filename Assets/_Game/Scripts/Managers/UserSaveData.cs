using UnityEngine.Scripting;
using TwoCore;
using System.Collections.Generic;
using System.Linq;

[Preserve]
public class UserSaveData : BaseUserData
{
    public static UserSaveData Ins => LocalData.Get<UserSaveData>();

    public bool SfxOn;
    public bool MusicOn;
    public bool VibrateOn;
    public int Coin;

    public int Level;

    public List<CharacterSaveData> CharacterSaveDatas;

    // Backward-compatible accessor used across gameplay code
    public CharacterSaveData CharacterSaveData => GetCharacterUse() ?? (CharacterSaveDatas != null && CharacterSaveDatas.Count > 0 ? CharacterSaveDatas[0] : null);

    protected internal override void OnInit()
    {
        base.OnInit();
        SfxOn = true;
        MusicOn = true;
        VibrateOn = true;
        CharacterSaveDatas = new List<CharacterSaveData>();
        Level = 1;
        Coin = 0;

        LoadCharacters();
    }

    protected internal override void OnLoad()
    {
        base.OnLoad();
        CharacterSaveDatas ??= new List<CharacterSaveData>();
        LoadCharacters();
    }

    public void LoadCharacters()
    {
        if (GameConfig.Ins?.CharaterDatas == null)
            return;

        // add missing entries
        foreach (var cfg in GameConfig.Ins.CharaterDatas)
        {
            if (!CharacterSaveDatas.Any(c => c.CharacterID == cfg.id))
            {
                CharacterSaveDatas.Add(new CharacterSaveData()
                {
                    CharacterID = cfg.id,
                    Level = 1,
                    IsUnlock = cfg.IsUnlock,
                    IsUse = cfg.IsUse,
                    CurrHP = cfg.HP,
                    CurrSpeed = cfg.Speed,
                    DamageLeft = cfg.DamgeLeftHand,
                    DamageRight = cfg.DamgeRightHand,
                });
            }
        }

        // remove entries that no longer exist in config
        CharacterSaveDatas.RemoveAll(s => !GameConfig.Ins.CharaterDatas.Any(c => c.id == s.CharacterID));

        // ensure only one IsUse
        int useCount = CharacterSaveDatas.Count(c => c.IsUse);
        if (useCount == 0 && CharacterSaveDatas.Count > 0)
            CharacterSaveDatas[0].IsUse = true;
        else if (useCount > 1)
        {
            bool kept = false;
            for (int i = 0; i < CharacterSaveDatas.Count; i++)
            {
                if (!CharacterSaveDatas[i].IsUse) continue;
                if (!kept) kept = true;
                else CharacterSaveDatas[i].IsUse = false;
            }
        }

        Save();
    }

    public CharacterSaveData GetCharacterUse()
    {
        return CharacterSaveDatas?.Find(_ => _.IsUse);
    }

    public CharacterSaveData GetCharacter(int id)
    {
        return CharacterSaveDatas?.Find(_ => _.CharacterID == id);
    }

    public void EquipCharacter(CharacterSaveData data)
    {
        if (CharacterSaveDatas == null || data == null) return;

        foreach (var item in CharacterSaveDatas)
            item.IsUse = false;

        var target = CharacterSaveDatas.Find(c => c.CharacterID == data.CharacterID);
        if (target != null) target.IsUse = true;

        SaveAndNotify("EquipCharacter");
    }

    public void NextLevel()
    {
        Level++;
        Save();
    }

    public void AddCoin(int value)
    {
        Coin += value;
        SaveAndNotify("coin");
    }

    public void UpdateCharacters()
    {
        LoadCharacters();
        SaveAndNotify("UpdateCharacters");
    }
}
