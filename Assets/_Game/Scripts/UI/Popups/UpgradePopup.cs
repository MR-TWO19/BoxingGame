using DG.Tweening;
using Doozy.Engine.UI;
using TMPro;
using TwoCore;
using UnityEngine;

public class UpgradePopup : BasePopup
{
    public static UpgradePopup Show()
    {
        return Show<UpgradePopup>("UpgradePopup", PopupShowMethod.QUEUE);
    }

    [Header("UI References")]
    [SerializeField] private UIButton btnUpgrade;
    [SerializeField] private UIButton btnClose;

    [Header("HP Upgrade")]
    [SerializeField] private TextMeshProUGUI txtCurrentHP;
    [SerializeField] private TextMeshProUGUI txtPrice;

    [Header("Speed Upgrade")]
    [SerializeField] private TextMeshProUGUI txtCurrentSpeed;

    [Header("Left Attack Upgrade")]
    [SerializeField] private TextMeshProUGUI txtCurrentLAtk;

    [Header("Right Attack Upgrade")]
    [SerializeField] private TextMeshProUGUI txtCurrentRAtk;

    [Header("Character Display")]
    [SerializeField] private Transform characterPos;

    [Header("Character")]
    [SerializeField] private Character character;

    private CharacterSaveData characterData;


    protected override void Awake()
    {
        base.Awake();

        if (btnUpgrade != null)
            btnUpgrade.OnClick.OnTrigger.Event.AddListener(UpgradeAll);
        btnClose.OnClick.OnTrigger.Event.AddListener(Hide);
    }

    private void OnEnable()
    {
        UpdateUI();
    }


    private void UpdateUI()
    {

        characterData = UserSaveData.Ins.CharacterSaveData;

        var upgradeData = GameConfig.Ins.UpdareData;

        float addHp = upgradeData.HP.value;
        float addSpeed = upgradeData.Speed.value;
        float addLAtk = upgradeData.LATK.value;
        float addRAtk = upgradeData.RATK.value;
        int price = upgradeData.HP.Price * UserSaveData.Ins.CharacterSaveData.Level;

        // HP
        if (txtCurrentHP != null)
            txtCurrentHP.text = $"{characterData.CurrHP:F1} <size=70%><color=#00FF00>+{addHp:F1}</color></size>";

        // Speed
        if (txtCurrentSpeed != null)
            txtCurrentSpeed.text = $"{characterData.CurrSpeed:F2} <size=70%><color=#00FF00>+{addSpeed:F2}</color></size>";

        // Left Attack
        if (txtCurrentLAtk != null)
            txtCurrentLAtk.text = $"{characterData.DamageLeft:F1} <size=70%><color=#00FF00>+{addLAtk:F1}</color></size>";

        // Right Attack
        if (txtCurrentRAtk != null)
            txtCurrentRAtk.text = $"{characterData.DamageRight:F1} <size=70%><color=#00FF00>+{addRAtk:F1}</color></size>";

        if (txtPrice != null)
            txtPrice.text = $"{price}";

        if (btnUpgrade != null)
            btnUpgrade.gameObject.SetActive(true);
    }

    private bool CanAfford(int price)
    {

        if (UserSaveData.Ins.Coin < price)
        {
            Hide();
            ConfirmPopup.ShowOneButtonNoQueue(
                "NOTIFY",
                "You do not have enough money to perform this action.",
                null,
                "OK"
            );
            return false;
        }
        return true;
    }

    private void UpgradeAll()
    {
        if (characterData == null || GameConfig.Ins.UpdareData == null)
            return;

        var upgradeData = GameConfig.Ins.UpdareData;
        int price = upgradeData.HP.Price * UserSaveData.Ins.CharacterSaveData.Level;
        float addHp = upgradeData.HP.value;
        float addSpeed = upgradeData.Speed.value;
        float addLAtk = upgradeData.LATK.value;
        float addRAtk = upgradeData.RATK.value;

        if (!CanAfford(price))
            return;

        SoundManager.Ins.PlayOneShot(SoundID.UPGRADE);

        characterData.Level += 1;
        characterData.CurrHP += addHp;
        characterData.CurrSpeed += addSpeed;
        characterData.DamageLeft += addLAtk;
        characterData.DamageRight += addRAtk;

        UserSaveData.Ins.AddCoin(-price);
        UserSaveData.Ins.Save();

        UpdateUI();
        AnimateUpgradeSuccess(txtCurrentHP);
        AnimateUpgradeSuccess(txtCurrentSpeed);
        AnimateUpgradeSuccess(txtCurrentLAtk);
        AnimateUpgradeSuccess(txtCurrentRAtk);

        PlayRandomUpgradeAnimation();
    }

    private void PlayRandomUpgradeAnimation()
    {
        if (character == null || character.animator == null)
            return;

        if (character.characterState != CharacterState.Idle)
            return;

        var anims = new[] {
            CharacterState.PunchLeft,
            CharacterState.PunchRight,
            CharacterState.PunchUppercut,
            CharacterState.PunchStraight
        };
        int idx = Random.Range(0, anims.Length);
        var chosen = anims[idx];

        character.characterState = chosen;
        character.animator.SetTrigger(chosen.ToString());

        float duration = GetClipDurationSafe(chosen);
        if (duration <= 0f) return;
        DOVirtual.DelayedCall(duration, () => {
            if (character == null || character.animator == null) return;
            character.animator.SetTrigger("Idle");
            character.characterState = CharacterState.Idle;
        });
    }

    private float GetClipDurationSafe(CharacterState state)
    {
        if (character == null || character.animator == null)
            return 0f;

        var controller = character.animator.runtimeAnimatorController;
        if (controller == null)
            return 0f;

        string clipName = state.ToString();
        var clips = controller.animationClips;
        if (clips == null)
            return 0f;

        for (int i = 0; i < clips.Length; i++) {
            var clip = clips[i];
            if (clip != null && clip.name == clipName)
                return clip.length;
        }

        return 0f;
    }

    private void AnimateUpgradeSuccess(TextMeshProUGUI text)
    {
        if (text == null)
            return;

        text.transform.DOKill();
        text.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f);

        var originalColor = text.color;
        text.DOColor(Color.green, 0.15f).OnComplete(() =>
        {
            text.DOColor(originalColor, 0.15f);
        });
    }
}
