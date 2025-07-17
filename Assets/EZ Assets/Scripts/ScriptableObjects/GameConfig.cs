using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObjectBase<GameConfig>
{
    public PowerExtraData PowerExtraDataPlayer;
    public PowerExtraData PowerExtraDataAlly;

    public int headDamageRate; // %
    public int bellyRate; // %

}