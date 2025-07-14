using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public SimpleJoystick joystick;
    [SerializeField] private Character character;

    private void Awake() => character = GetComponent<Character>();

    private void Start()
    {
        SwipeDetector.Ins.OnSwipeUp.AddListener(() => character.Attack(CharacterState.PunchUppercut));
        SwipeDetector.Ins.OnSwipeDown.AddListener(() => character.Dodge());
        SwipeDetector.Ins.OnSwipeLeft.AddListener(() => character.Attack(CharacterState.PunchLeft));
        SwipeDetector.Ins.OnSwipeRight.AddListener(() => character.Attack(CharacterState.PunchRight));
        SwipeDetector.Ins.OnSwipeClick.AddListener(() => character.Attack(CharacterState.PunchStraight));
    }

    void Update()
    {
        character.ControlByDirection(joystick.Direction());
    }
}
