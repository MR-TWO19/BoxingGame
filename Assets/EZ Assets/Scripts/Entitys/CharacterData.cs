using System;

[Serializable]
public class CharacterData
{
    public string Name;
    public float HP = 10;
    public float Speed = 0.1f;
    public float DamgeLeftHand = 1f;
    public float DamgeRightHand = 2f;

    public CharacterData(string name, float hp, float speed, float damgeLeftHand, float damgeRightHand)
    {
        Name = name;
        HP = hp;
        Speed = speed;
        DamgeLeftHand = damgeLeftHand;
        DamgeRightHand = damgeRightHand;
    }
}
