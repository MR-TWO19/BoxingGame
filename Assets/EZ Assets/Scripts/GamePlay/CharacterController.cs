using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CharacterController : MonoBehaviour
{
    public SimpleJoystick joystick;
    public float speed = 5f;
    [SerializeField] Animator animator;

    private CharacterState characterState = CharacterState.Idle;
    private bool IsAction;

    void Update()
    {
        ControlCharacter();
    }

    private void Start()
    {
        SwipeDetector.Ins.OnSwipeUp.AddListener(() => { Attack(CharacterState.PunchUppercut); });
        SwipeDetector.Ins.OnSwipeDown.AddListener(Dodge);
        SwipeDetector.Ins.OnSwipeLeft.AddListener(() => { Attack(CharacterState.PunchLeft); });
        SwipeDetector.Ins.OnSwipeRight.AddListener(() => { Attack(CharacterState.PunchRight); });
        SwipeDetector.Ins.OnSwipeClick.AddListener(() => { Attack(CharacterState.PunchStraight); });
    }

    private void ControlCharacter()
    {
        if (characterState == CharacterState.PunchUppercut || characterState == CharacterState.PunchLeft ||
            characterState == CharacterState.PunchRight || characterState == CharacterState.PunchStraight ||
            characterState == CharacterState.Dodge)
            return;

        Vector2 dir = joystick.Direction();
        Vector3 move = new(dir.x, 0, dir.y);
        transform.Translate(speed * Time.deltaTime * move, Space.World);
        if (move.sqrMagnitude > 0.01f)
        {
            if (characterState != CharacterState.Move)
            {
                characterState = CharacterState.Move;
                animator.SetTrigger("moving");
            }
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
        else
        {
            if (characterState != CharacterState.Idle)
            {
                characterState = CharacterState.Idle;
                animator.SetTrigger("Idle");
            }

        }
    }

    private void Attack(CharacterState State)
    {
        if (IsAction) return;
        IsAction = true;
        characterState = State;
        switch (characterState)
        {
            case CharacterState.PunchLeft:
                animator.SetTrigger("PunchLeft");
                break;
            case CharacterState.PunchRight:
                animator.SetTrigger("PunchRight");
                break;
            case CharacterState.PunchUppercut:
                animator.SetTrigger("PunchUppercut");
                break;
            case CharacterState.PunchStraight:
                animator.SetTrigger("PunchStraight");
                break;
            default:
                break;
        }

        StartCoroutine(ResetToIdleAfterAnimation(State));
    }

    private void Dodge()
    {
        characterState = CharacterState.Dodge;
        animator.SetTrigger("Dodge");
        StartCoroutine(ResetToIdleAfterAnimation(CharacterState.Dodge));
    }

    private IEnumerator ResetToIdleAfterAnimation(CharacterState state)
    {
        string clipName = state.ToString();
        float clipduration = GetClipDuration(clipName);
        float duration = clipduration - (clipduration * 0.25f);
        yield return new WaitForSeconds(duration);
        IsAction = false;
        characterState = CharacterState.Idle;
    }

    private float GetClipDuration(string clipName)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        Debug.LogWarning("not found clip: " + clipName);
        return 0f;
    }
}
