using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ConfigBase<GameConfig>
{
    public int headDamageRate; // %
    public int bellyRate; // %

    public GameObject ArenaPrefab;

    public UpdareData UpdareData;

    public List<CharacterData> CharaterDatas;

    public CharacterData GetCharacterData(int id)
    {
        return CharaterDatas.Find(c => c.id == id);
    }

}