using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData characterData = new(10,1,1,2);
    public Animator animator;
    public HitBox headHitBox;
    public HitBox bellyHitBox;

    private CharacterState characterState = CharacterState.Idle;
    private TeamType teamType;
    private bool isAction;

    private void Start()
    {
        headHitBox.character = this;
        headHitBox.OnhitEvent.AddListener(HeadHitBoxEvent);
        bellyHitBox.character = this;
        bellyHitBox.OnhitEvent.AddListener(BellyHitBoxEvent);
    }

    #region Public Methods
    public void ControlByDirection(Vector2 dir)
    {
        if (IsInAction()) return;

        Vector3 move = new(dir.x, 0, dir.y);
        transform.Translate(characterData.Speed * Time.deltaTime * move, Space.World);

        if (move.sqrMagnitude > 0.01f)
        {
            if (characterState != CharacterState.Move)
            {
                characterState = CharacterState.Move;
                //animator.SetTrigger("moving");
                animator.SetBool("Move", true);
                Debug.Log("B--- moving");
            }

            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
        else
        {
            if (characterState != CharacterState.Idle)
            {
                characterState = CharacterState.Idle;
                //animator.SetTrigger("Idle");
                animator.SetBool("Move", false);
            }
        }
    }

    public void Attack(CharacterState state)
    {
        if (isAction) return;
        isAction = true;
        characterState = state;
        animator.SetTrigger(state.ToString());
        StartCoroutine(ResetToIdleAfterAnimation(state));
    }

    public void Dodge()
    {
        if (isAction) return;
        isAction = true;
        characterState = CharacterState.Dodge;
        animator.SetTrigger("Dodge");
        StartCoroutine(ResetToIdleAfterAnimation(CharacterState.Dodge));
    }
    #endregion

    #region UnityEvent
    private void HeadHitBoxEvent(HitBox hitBox, Character character)
    {
        if (hitBox != null && hitBox.teamType == teamType)
        {
            Damage(BodyPart.Head, hitBox.bodyPart, character.characterData.DamgeRightHand);
        }
        Debug.Log("B---- HeadHitBoxEvent");
    }

    private void BellyHitBoxEvent(HitBox hitBox, Character character)
    {
        if (character.characterState == CharacterState.PunchUppercut) return;
        if (hitBox != null && hitBox.teamType == teamType)
        {

        }

        Debug.Log("B---- BellyHitBoxEvent");
    }
    #endregion

    #region private halpers
    private bool IsInAction()
    {
        return characterState == CharacterState.PunchUppercut ||
               characterState == CharacterState.PunchLeft ||
               characterState == CharacterState.PunchRight ||
               characterState == CharacterState.PunchStraight ||
               characterState == CharacterState.Dodge;
    }

    private IEnumerator ResetToIdleAfterAnimation(CharacterState state)
    {
        string clipName = state.ToString();
        float duration = GetClipDuration(clipName) * 0.9f;
        yield return new WaitForSeconds(duration);
        isAction = false;
        characterState = CharacterState.Idle;
        animator.SetTrigger("Idle");
    }

    private float GetClipDuration(string clipName)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        Debug.LogWarning("Not found animation clip: " + clipName);
        return 0f;
    }

    private void Damage(BodyPart hitPart, BodyPart attackerPart, float damage)
    {
        characterState = CharacterState.Dodge;

    }
    #endregion
}
