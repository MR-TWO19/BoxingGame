using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModeGameConfig", menuName = "Config/ModeGameConfig")]
public class ModeGameConfig : ScriptableObjectBase<ModeGameConfig>
{
    public GameModeData OneVsOneMode;
    public GameModeData OneVsManyMode;
    public GameModeData ManyVsManyMode;
}
