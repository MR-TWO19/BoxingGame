using UnityEngine.Scripting;
using TwoCore;
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

    public CharacterSaveData CharacterSaveData;

    protected internal override void OnInit()
    {
        base.OnInit();
        SfxOn = true;
        MusicOn = true;
        VibrateOn = true;
        CharacterSaveData = new CharacterSaveData();
        Level = 1;
        Coin = 0;

        LoadCharacter();
    }

    protected internal override void OnLoad()
    {
        base.OnLoad();
        CharacterSaveData ??= new CharacterSaveData();
        LoadCharacter();
    }

    // Sync single save data with current GameConfig characters
    public void LoadCharacter()
    {
        if (GameConfig.Ins?.CharaterDatas == null || GameConfig.Ins.CharaterDatas.Count == 0)
            return;

        CharacterSaveData ??= new CharacterSaveData();

        var cfg = GameConfig.Ins.CharaterDatas.FirstOrDefault(c => c.id == CharacterSaveData.CharacterID)
                  ?? GameConfig.Ins.CharaterDatas[0];

        CharacterSaveData.CharacterID = cfg.id;

        // Initialize current stats if needed
        if (CharacterSaveData.Level <= 0) CharacterSaveData.Level = 1;
        if (CharacterSaveData.CurrHP <= 0) CharacterSaveData.CurrHP = cfg.HP;
        if (CharacterSaveData.CurrSpeed <= 0) CharacterSaveData.CurrSpeed = cfg.Speed;
        if (CharacterSaveData.DamageLeft <= 0) CharacterSaveData.DamageLeft = cfg.DamgeLeftHand;
        if (CharacterSaveData.DamageRight <= 0) CharacterSaveData.DamageRight = cfg.DamgeRightHand;

        Save();
    }

    public CharacterSaveData GetCharacterUse()
    {
        return CharacterSaveData != null && CharacterSaveData.IsUse ? CharacterSaveData : null;
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

    // Force update/sync with GameConfig (call after changing GameConfig in editor)
    public void UpdateCharacter()
    {
        LoadCharacter();
        SaveAndNotify("UpdateCharacter");
    }
}
