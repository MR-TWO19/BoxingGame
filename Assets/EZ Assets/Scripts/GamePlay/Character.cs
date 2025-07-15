using System.Collections;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class Character : MonoBehaviour
{
    public CharacterData characterData = new(10,1,1,2);
    public Animator animator;
    public HitBox headHitBox;
    public HitBox bellyHitBox;
    public HandHitBox rightHandHitBox;
    public HandHitBox leftHandHitBox;

    public CharacterState characterState = CharacterState.Idle;
    private TeamType teamType;
    private bool isAction;

    private void Start()
    {
        headHitBox.character = this;
        bellyHitBox.character = this;
        rightHandHitBox.character = this;
        leftHandHitBox.character = this;
        rightHandHitBox.DissableColide();
        leftHandHitBox.DissableColide();
        headHitBox.OnhitEvent.AddListener(HeadHitBoxEvent);
        bellyHitBox.OnhitEvent.AddListener(BellyHitBoxEvent);
    }

    private void UpdateAtkTarget()
    {
        //Vector3 targetPos = target.transform.position;
        //float distance = Vector3.Distance(transform.position, targetPos);
        //float distanceATKTagert = distanceAtk;
        //if (distance > distanceATKTagert && isChasing)
        //{
        //    MoveCharacter(true);
        //    transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        //    transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
        //}
        //else if (!isAtk)
        //{
        //    isAtk = true;
        //    DOVirtual.DelayedCall(0.5f, () =>
        //    {
        //        isAtk = false;
        //    });
        //    Attack();
        //    transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
        //    isChasing = Vector3.Distance(transform.position, target.transform.position) > distanceATKTagert;
        //}
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
        if(characterState == CharacterState.PunchLeft)
            leftHandHitBox.EnabledColide();
        else if(characterState == CharacterState.PunchRight || characterState == CharacterState.PunchUppercut || characterState == CharacterState.PunchStraight)
            rightHandHitBox.EnabledColide();

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
    private void HeadHitBoxEvent(HitBox hitBox)
    {
        if (hitBox != null && hitBox.teamType == teamType)
        {
            float damage;
            CharacterState characterState;
            if (hitBox.CompareTag("RightHand") && hitBox.character.characterState == CharacterState.PunchUppercut)
            {
                damage = hitBox.character.characterData.DamgeRightHand * (GameConfig.Ins.headDamageRate / 100);
                characterState = CharacterState.HeadHit;
                hitBox.character.rightHandHitBox.DissableColide();
            }
            else
            {
                hitBox.character.rightHandHitBox.DissableColide();
                return;
            }
            TakeDamage(damage);
            animator.SetTrigger(characterState.ToString());
            StartCoroutine(ResetToIdleAfterAnimation(characterState));
        }
    }

    private void BellyHitBoxEvent(HitBox hitBox)
    {
        if (hitBox.character.characterState == CharacterState.PunchUppercut) return;
        if (hitBox != null && hitBox.teamType == teamType)
        {
            float damage;
            CharacterState characterState;
            if (hitBox.CompareTag("LeftHand") && hitBox.character.characterState == CharacterState.PunchLeft)
            {
                damage = hitBox.character.characterData.DamgeLeftHand * (GameConfig.Ins.bellyRate / 100);
                characterState = CharacterState.KidneyHitLeft;
                hitBox.character.leftHandHitBox.DissableColide();
            }
            else if (hitBox.CompareTag("RightHand") && hitBox.character.characterState == CharacterState.PunchRight)
            {
                damage = hitBox.character.characterData.DamgeRightHand * (GameConfig.Ins.bellyRate / 100);
                characterState = CharacterState.KidneyHitRight;
                hitBox.character.rightHandHitBox.DissableColide();
            }
            else
            {
                hitBox.character.rightHandHitBox.DissableColide();
                hitBox.character.leftHandHitBox.DissableColide();
                return;
            }
            TakeDamage(damage);
            animator.SetTrigger(characterState.ToString());
            StartCoroutine(ResetToIdleAfterAnimation(characterState));
        }
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
        string clipName = (state == CharacterState.PunchLeft || state == CharacterState.PunchRight) ? "KidneyHit" : state.ToString();
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

    private void TakeDamage(float damage)
    {
        characterState = CharacterState.Dodge;
        characterData.Head -= damage;
    }
    #endregion
}
