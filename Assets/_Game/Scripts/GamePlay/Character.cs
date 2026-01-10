using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData OriginCharacterData = new("Character", 10,0.1f,1,2);
    public CharacterData CurCharacterData;
    public TextMeshPro txtName;
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
    private float currHP;
    private List<CharacterState> useSkills = new();

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
    public void SetUp(string name, PowerExtraData data, TeamType _teamType, List<CharacterState> useSkills)
    {
        txtName.text = name;
        teamType = _teamType;
        this.useSkills.Clear();
        this.useSkills.AddRange(useSkills);
        if (name == "Player")
            txtName.color = Color.green;
        else if (teamType == TeamType.Enemy)
            txtName.color = Color.red;
        else
            txtName.color = Color.yellow;

        rigidbodyCharacter.isKinematic = false;
        ColliderCharacter.isTrigger = false;

        CurCharacterData = new CharacterData(OriginCharacterData);

        CurCharacterData.HP += data.HP;
        CurCharacterData.Speed += data.Speed;
        CurCharacterData.DamgeRightHand += data.DamgeRightHand;
        CurCharacterData.DamgeLeftHand += data.DamgeLeftHand;
        gameObject.tag = teamType.ToString();
        currHP = CurCharacterData.HP;

        if (teamType == TeamType.Ally)
            InGameView.Ins.SetUpHelthAlly(txtName.text, CurCharacterData.HP, CurCharacterData.HP, CurCharacterData.HP);
        else
            InGameView.Ins.SetUpHelthEnemy(txtName.text, CurCharacterData.HP, CurCharacterData.HP, CurCharacterData.HP);

        characterState = CharacterState.Idle;
    }

    public void ControlByDirection(Vector2 dir)
    {
        if (IsInAction()) return;

        Vector3 move = new(dir.x, 0, dir.y);
        transform.Translate(CurCharacterData.Speed * Time.deltaTime * move, Space.World);

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
        transform.position = Vector3.MoveTowards(transform.position, targetPos, CurCharacterData.Speed * Time.deltaTime);
        transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
    }

    public void BotAttack(Vector3 targetPos)
    {
        if (isAction || IsDamage()) return;
        transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
        animator.SetBool("Move", false);
        int idx = Random.Range(0, useSkills.Count);
        Attack(useSkills[idx]);

    }

    public void BotDisible()
    {
        animator.SetTrigger("Idle");
    }

    public void Attack(CharacterState state)
    {
        if (isAction || IsDamage()) return;
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

    public void Victory()
    {
        animator.SetTrigger("Victory");
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
                damage = hitBox.character.CurCharacterData.DamgeRightHand * (1f + GameConfig.Ins.headDamageRate / 100);
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
                damage =  hitBox.character.CurCharacterData.DamgeLeftHand * (1f + (float)GameConfig.Ins.bellyRate / 100);
                characterState = CharacterState.KidneyHitLeft;
                hitBox.character.leftHandHitBox.DissableColide();
            }
            else if (hitBox.CompareTag("RightHand") && hitBox.character.characterState == CharacterState.PunchRight)
            {
                damage = hitBox.character.CurCharacterData.DamgeRightHand * (1f + ((float)GameConfig.Ins.bellyRate / 100));
                characterState = CharacterState.KidneyHitRight;
                hitBox.character.rightHandHitBox.DissableColide();
            }
            else if (hitBox.CompareTag("RightHand") && hitBox.character.characterState == CharacterState.PunchStraight)
            {
                damage = hitBox.character.CurCharacterData.DamgeRightHand;
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

    private bool IsDamage()
    {
        return characterState is CharacterState.HeadHit or
               CharacterState.KidneyHitLeft or
               CharacterState.KidneyHitRight or
               CharacterState.StomachHit;
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
        float oldHP = currHP;
        currHP -= damage;

        if (currHP <= 0)
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
            if (teamType == TeamType.Ally)
                GameManager.Ins.GameMove.AllyDead(this);
            else
                GameManager.Ins.GameMove.EnemyDead(this);
        }
        if (teamType == TeamType.Ally)
            InGameView.Ins.SetUpHelthAlly(txtName.text, CurCharacterData.HP, oldHP, currHP);
        else
            InGameView.Ins.SetUpHelthEnemy(txtName.text, CurCharacterData.HP, oldHP, currHP);
    }

    #endregion
}
