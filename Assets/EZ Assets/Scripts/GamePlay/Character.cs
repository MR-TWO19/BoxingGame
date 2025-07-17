using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData characterData = new(10,0.1f,1,2);
    public Animator animator;
    public HitBox headHitBox;
    public HitBox bellyHitBox;
    public HandHitBox rightHandHitBox;
    public HandHitBox leftHandHitBox;
    public Rigidbody  rigidbodyCharacter;
    public Collider  ColliderCharacter;

    public CharacterState characterState = CharacterState.Idle;
    public TeamType teamType;
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

    #region Public Methods
    public void SetUp(CharacterData data, TeamType _teamType)
    {
        rigidbodyCharacter.isKinematic = false;
        ColliderCharacter.isTrigger = false;
        characterData.HP += data.HP;
        characterData.Speed += data.Speed;
        characterData.DamgeRightHand += data.DamgeRightHand;
        characterData.DamgeLeftHand += data.DamgeLeftHand;
        teamType = _teamType;
        gameObject.tag = teamType.ToString();
    }

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

    public void BotMove(Vector3 targetPos)
    {
        animator.SetBool("Move", true);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, characterData.Speed * Time.deltaTime);
        transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
    }

    public void BotAttack()
    {
        if (isAction) return;
        animator.SetBool("Move", false);
        Attack(CharacterState.PunchStraight);

    }

    public void BotDisible()
    {
        animator.SetTrigger("Idle");
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

    public bool IsKnockedOut()
    {
        return characterState == CharacterState.KnockedOut;
    }
    #endregion

    #region UnityEvent
    private void HeadHitBoxEvent(HitBox hitBox)
    {
        if (characterState == CharacterState.Dodge || IsKnockedOut()) return;
        if (hitBox != null && hitBox.character.teamType != teamType)
        {
            float damage;
            if (hitBox.CompareTag("RightHand") && hitBox.character.characterState == CharacterState.PunchUppercut)
            {
                damage = hitBox.character.characterData.DamgeRightHand * (GameConfig.Ins.headDamageRate / 100);
                characterState = CharacterState.HeadHit;
                hitBox.character.rightHandHitBox.DissableColide();
            }
            else
            {
                return;
            }

            TakeDamage(damage);
            animator.SetTrigger(characterState.ToString());
            StartCoroutine(ResetToIdleAfterAnimation(characterState));
        }
    }

    private void BellyHitBoxEvent(HitBox hitBox)
    {
        if (hitBox.character.characterState == CharacterState.PunchUppercut || characterState == CharacterState.Dodge || IsKnockedOut()) return;
        if (hitBox != null && hitBox.character.teamType != teamType)
        {
            float damage;
            if (hitBox.CompareTag("LeftHand") && hitBox.character.characterState == CharacterState.PunchLeft)
            {
                damage =  hitBox.character.characterData.DamgeLeftHand * (1f + (float)GameConfig.Ins.bellyRate / 100);
                characterState = CharacterState.KidneyHitLeft;
                hitBox.character.leftHandHitBox.DissableColide();
            }
            else if (hitBox.CompareTag("RightHand") && hitBox.character.characterState == CharacterState.PunchRight)
            {
                damage = hitBox.character.characterData.DamgeRightHand * (1f + ((float)GameConfig.Ins.bellyRate / 100));
                characterState = CharacterState.KidneyHitRight;
                hitBox.character.rightHandHitBox.DissableColide();
            }
            else if (hitBox.CompareTag("RightHand") && hitBox.character.characterState == CharacterState.PunchStraight)
            {
                damage = hitBox.character.characterData.DamgeRightHand;
                characterState = CharacterState.StomachHit;
                hitBox.character.rightHandHitBox.DissableColide();
            }
            else
            {
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
        return characterState is CharacterState.PunchUppercut or
               CharacterState.PunchLeft or
               CharacterState.PunchRight or
               CharacterState.PunchStraight or
               CharacterState.HeadHit or
               CharacterState.KidneyHitLeft or
               CharacterState.PunchStraight or
               CharacterState.KidneyHitRight or
               CharacterState.StomachHit or
               CharacterState.Dodge;
    }

    private IEnumerator ResetToIdleAfterAnimation(CharacterState state)
    {
        string clipName = (state == CharacterState.KidneyHitLeft || state == CharacterState.KidneyHitRight) ? "KidneyHit" : state.ToString();
        float duration = GetClipDuration(clipName);
        yield return new WaitForSeconds(duration);
        isAction = false;
        if (!IsKnockedOut())
        {
            characterState = CharacterState.Idle;
            animator.SetTrigger("Idle");
        }
        rightHandHitBox.DissableColide();
        leftHandHitBox.DissableColide();
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
        characterData.HP -= damage;
        if(characterData.HP <= 0)
        {
            rigidbodyCharacter.isKinematic = true;
            ColliderCharacter.isTrigger = true;
            characterState = CharacterState.KnockedOut;
            animator.SetTrigger(characterState.ToString());
            float duration = GetClipDuration(characterState.ToString()) + 2;
            DOVirtual.DelayedCall(duration, () =>
            {
                gameObject.SetActive(false);
            });

        }    
    }

    #endregion
}
