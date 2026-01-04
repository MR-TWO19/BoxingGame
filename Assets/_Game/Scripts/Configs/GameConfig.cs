using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ConfigBase<GameConfig>
{
    public int headDamageRate; // %
    public int bellyRate; // %


    public UpdareData UpdareData;

    public SkillData QuantitySlotSkill;
    public SkillData ImmunitySkill;
    public SkillData ExtraRollSkill;

    public SkillData AccelerateSkill;
    public SkillData KnockbackSkill;
    public SkillData SlowSkill;
    public SkillData PowerSkill;

    public List<CarData> CarDatas;

    public CarData GetCar(int id)
    {
        return CarDatas.Find(_ => _.id == id);
    }
}