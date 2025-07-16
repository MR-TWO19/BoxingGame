using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObjectBase<GameConfig>
{
    public CharacterData characterDataPlayer;
    public CharacterData characterDataAlly;
    public CharacterData characterDataEnemy;

    public int headDamageRate; // %
    public int bellyRate; // %

}