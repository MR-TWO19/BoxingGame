public enum GameMode
{
    OneVSOne,
    OneVSMany,
    ManeyVsMany
} 


#region Character
public enum CharacterState
{
    Idle,
    Move, 
    PunchLeft,
    PunchRight, 
    PunchUppercut,
    PunchStraight,
    Dodge,
    HeadHit,
    KidneyHitLeft,
    KidneyHitRight,
    StomachHit,
    KnockedOut,
}

public enum BodyPart
{
    Head,
    LeftHand,
    RightHand,
    Belly
}

public enum TeamType
{
    Ally,
    Enemy
}
#endregion