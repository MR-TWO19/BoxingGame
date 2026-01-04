using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObjectBase<GameConfig>
{
    public int headDamageRate; // %
    public int bellyRate; // %

}