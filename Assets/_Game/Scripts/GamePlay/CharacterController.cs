using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Character character;
    private Character objTarget;
    private bool isActive;
    private bool isBot;
    private bool botDisable;
    private bool isAtk;
    private float timeBotDisible = 5;
    private void Awake() => character = GetComponent<Character>();

    public void CustomUpdate()
    {
        if(character.IsKnockedOut() || !isActive) return;

        if (!isBot)
        {
           if(UIManager.Ins.joystick) character.ControlByDirection(UIManager.Ins.joystick.Direction());
        }
        else
        {
            BotHandler();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (!isBot || character.IsKnockedOut() || !isActive) return;
        if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Ally") ||
            collision.gameObject.CompareTag("Ally") && gameObject.CompareTag("Enemy"))
        {
            isAtk = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isBot || character.IsKnockedOut() || !isActive) return;
        if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Ally") ||
            collision.gameObject.CompareTag("Ally") && gameObject.CompareTag("Enemy"))
        {
            isAtk = true;
        }
        else
        {
            botDisable = true;
            DOVirtual.DelayedCall(timeBotDisible, () =>
            {
                botDisable = false;
            });
        }    
    }

    public void SetUp(string name, PowerExtraData extraData, TeamType teamType, bool isPlayer, List<CharacterState> useSkills)
    {
        isActive = false;
        botDisable = false;
        isAtk = false;
        if (isPlayer)
        {
            SwipeDetector.Ins.OnSwipeUp.AddListener(() => character.Attack(CharacterState.PunchUppercut));
            SwipeDetector.Ins.OnSwipeDown.AddListener(() => character.Dodge());
            SwipeDetector.Ins.OnSwipeLeft.AddListener(() => character.Attack(CharacterState.PunchLeft));
            SwipeDetector.Ins.OnSwipeRight.AddListener(() => character.Attack(CharacterState.PunchRight));
            SwipeDetector.Ins.OnSwipeClick.AddListener(() => character.Attack(CharacterState.PunchStraight));
            CameraManager.Ins.SetUpCammeraFollow(gameObject);
        }
        isBot = !isPlayer;
        character.SetUp(name, extraData, teamType, useSkills);
    }

    public void Active()
    {
        isActive = true;

        if(isBot)
        {
            StartCoroutine(UpdateTagert());
        }    
    }

    IEnumerator UpdateTagert()
    {
        List<CharacterController> Characters = character.teamType == TeamType.Enemy ? GameManager.Ins.GameMove.Allys : GameManager.Ins.GameMove.Enemys;
        while (true)
        {
            CharacterController characterController = FindClosestTarget(Characters);
            if(characterController) objTarget = characterController.character;
            yield return new WaitForSeconds(3f);
        }
    }

    public CharacterController FindClosestTarget(List<CharacterController> targets)
    {
        CharacterController closest = null;
        float minDistance = Mathf.Infinity;

        foreach (var obj in targets)
        {
            float dist = Vector3.Distance(obj.transform.position, transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = obj;
            }
        }

        return closest;
    }

    private void BotHandler()
    {
        if(objTarget.IsKnockedOut() || botDisable) return;
        Vector3 targetPos = objTarget.transform.position;
        if (isAtk)
        {
            botDisable = true;
            character.BotAttack(targetPos);
            DOVirtual.DelayedCall(timeBotDisible, () =>
            {
                botDisable = false;
            });
        }
        else
        {
            character.BotMove(targetPos);
        }
    }
}
