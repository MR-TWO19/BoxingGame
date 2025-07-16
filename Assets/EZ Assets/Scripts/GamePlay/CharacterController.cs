using DG.Tweening;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public SimpleJoystick joystick;
    public Character objTarget;
    [SerializeField] private Character character;
    public bool isBot;
    private bool botDisable;
    private bool isAtk;
    private void Awake() => character = GetComponent<Character>();

    private void Start()
    {
        if (isBot) return;
        SwipeDetector.Ins.OnSwipeUp.AddListener(() => character.Attack(CharacterState.PunchUppercut));
        SwipeDetector.Ins.OnSwipeDown.AddListener(() => character.Dodge());
        SwipeDetector.Ins.OnSwipeLeft.AddListener(() => character.Attack(CharacterState.PunchLeft));
        SwipeDetector.Ins.OnSwipeRight.AddListener(() => character.Attack(CharacterState.PunchRight));
        SwipeDetector.Ins.OnSwipeClick.AddListener(() => character.Attack(CharacterState.PunchStraight));
    }

    void Update()
    {
        if(character.IsKnockedOut()) return;

        if (!isBot)
        {
           if(joystick) character.ControlByDirection(joystick.Direction());
        }
        else
        {
            BotHandler();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (!isBot || character.IsKnockedOut()) return;
        if (collision.gameObject.CompareTag("Enamy") && gameObject.CompareTag("Ally") ||
            collision.gameObject.CompareTag("Ally") && gameObject.CompareTag("Enamy"))
        {
            isAtk = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isBot || character.IsKnockedOut()) return;
        if (collision.gameObject.CompareTag("Enamy") && gameObject.CompareTag("Ally") ||
            collision.gameObject.CompareTag("Ally") && gameObject.CompareTag("Enamy"))
        {
            isAtk = true;
        }
    }

    private void BotHandler()
    {
        if(objTarget.IsKnockedOut() || botDisable) return;
        Vector3 targetPos = objTarget.transform.position;
        if (!isAtk)
        {
            character.BotMove(targetPos);
        }
        else
        {
            botDisable = true;
            character.BotAttack();
            DOVirtual.DelayedCall(5, () =>
            {
                botDisable = false;
            });
        }
    }
}
