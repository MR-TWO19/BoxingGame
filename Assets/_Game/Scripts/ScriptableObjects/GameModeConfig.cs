using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModeConfig", menuName = "Config/GameModeConfig")]
public class GameModeConfig : ScriptableObjectBase<GameModeConfig>
{
    public GameModeData OneVsOneMode;
    public GameModeData OneVsManyMode;
    public GameModeData ManyVsManyMode;
}
