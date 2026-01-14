using DG.Tweening;
using Doozy.Engine.UI;
using System.Collections.Generic;
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
    [SerializeField] private GameObject frameBuy;
    [SerializeField] private GameObject frameUpgrade;

    [SerializeField] private UIButton btnBuy;
    [SerializeField] private UIButton btnUpgrade;
    [SerializeField] private UIButton btnEquip;
    [SerializeField] private UIButton btnNextLeft;
    [SerializeField] private UIButton btnNextRight;
    [SerializeField] private UIButton btnClose;

    [Header("Texts")]
    //[SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private TextMeshProUGUI txtPriceBuy;
    [SerializeField] private TextMeshProUGUI txtPriceUpgrade;

    [SerializeField] private TextMeshProUGUI txtHP;
    [SerializeField] private TextMeshProUGUI txtSpeed;
    [SerializeField] private TextMeshProUGUI txtLAtk;
    [SerializeField] private TextMeshProUGUI txtRAtk;

    [Header("Character Display")]
    [SerializeField] private Transform characterPos;

    private readonly Dictionary<int, GameObject> objCharacters = new();
    private readonly Dictionary<int, CharacterSaveData> currSaveDatas = new();

    private int idxCharacter;
    private int priceUpgrade;

    protected override void Awake()
    {
        base.Awake();

        if (btnNextLeft != null)
            btnNextLeft.OnClick.OnTrigger.Event.AddListener(() => NextOnClick(true));

        if (btnNextRight != null)
            btnNextRight.OnClick.OnTrigger.Event.AddListener(() => NextOnClick(false));

        if (btnBuy != null)
            btnBuy.OnClick.OnTrigger.Event.AddListener(BuyOnClick);

        if (btnUpgrade != null)
            btnUpgrade.OnClick.OnTrigger.Event.AddListener(UpgradeOnClick);

        if (btnEquip != null)
            btnEquip.OnClick.OnTrigger.Event.AddListener(EquipOnClick);

        if (btnClose != null)
            btnClose.OnClick.OnTrigger.Event.AddListener(Hide);
    }

    private void OnEnable()
    {
        LoadCharacters();
    }

    private void LoadCharacters()
    {
        var saveDatas = UserSaveData.Ins.CharacterSaveDatas;
        if (saveDatas == null || saveDatas.Count == 0)
            return;

        foreach (var c in objCharacters.Values)
            if (c != null) Destroy(c);

        objCharacters.Clear();
        currSaveDatas.Clear();

        for (int i = 0; i < saveDatas.Count; i++)
        {
            var save = saveDatas[i];
            var data = GameConfig.Ins.GetCharacterData(save.CharacterID);
            if (data == null || data.Prefab == null) continue;

            GameObject obj = Instantiate(data.Prefab, characterPos);
            Character character = obj.GetComponent<Character>();
            character.OffColider();

            obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 180, 0));
            obj.transform.localScale = Vector3.zero;
            obj.SetActive(false);

            objCharacters.Add(i, obj);
            currSaveDatas.Add(i, save);
        }

        var usingChar = UserSaveData.Ins.GetCharacterUse() ?? saveDatas[0];
        idxCharacter = saveDatas.IndexOf(usingChar);
        if (idxCharacter < 0) idxCharacter = 0;

        btnNextLeft.gameObject.SetActive(idxCharacter != 0);
        btnNextRight.gameObject.SetActive(idxCharacter != saveDatas.Count - 1);

        if (objCharacters.TryGetValue(idxCharacter, out var chObj) && chObj != null)
        {
            chObj.SetActive(true);
            chObj.transform.DOScale(Vector3.one * 500, 0.35f);
        }

        ShowState(currSaveDatas[idxCharacter]);
    }

    private void ShowState(CharacterSaveData save)
    {
        if (save == null) return;

        if (save.IsUnlock)
            ShowUpgrade(save);
        else
            ShowBuy(save);
    }

    private void ShowUpgrade(CharacterSaveData save)
    {
        var upgrade = GameConfig.Ins.UpdareData;
        priceUpgrade = save.Level * (upgrade?.HP?.Price ?? 0);

        //if (txtLevel != null) txtLevel.text = $"Level {save.Level}";

        if (upgrade != null)
        {
            if (txtHP != null)
                txtHP.text = $"{save.CurrHP:0.0#} <size=70%><color=#00FF00>+{upgrade.HP.value:0.0#}</color></size>";
            if (txtSpeed != null)
                txtSpeed.text = $"{save.CurrSpeed:0.0#} <size=70%><color=#00FF00>+{upgrade.Speed.value:0.0#}</color></size>";
            if (txtLAtk != null)
                txtLAtk.text = $"{save.DamageLeft:0.0#} <size=70%><color=#00FF00>+{upgrade.LATK.value:0.0#}</color></size>";
            if (txtRAtk != null)
                txtRAtk.text = $"{save.DamageRight:0.0#} <size=70%><color=#00FF00>+{upgrade.RATK.value:0.0#}</color></size>";
        }

        if (txtPriceUpgrade != null)
            txtPriceUpgrade.text = $"{priceUpgrade}";

        if (frameBuy != null) frameBuy.SetActive(false);
        if (frameUpgrade != null) frameUpgrade.SetActive(true);

        if (btnEquip != null)
            btnEquip.gameObject.SetActive(!save.IsUse);
    }

    private void ShowBuy(CharacterSaveData save)
    {
        // No character price configured yet; reuse upgrade price as fallback to avoid null UI

        int priceBuy = GameConfig.Ins.GetCharacterData(save.CharacterID).Price; ;

        if (txtPriceBuy != null)
            txtPriceBuy.text = $"{priceBuy}";

        if (frameBuy != null) frameBuy.SetActive(true);
        if (frameUpgrade != null) frameUpgrade.SetActive(false);

        if (btnEquip != null)
            btnEquip.gameObject.SetActive(false);
    }

    private void NextOnClick(bool isLeft)
    {
        if (!objCharacters.TryGetValue(idxCharacter, out var currentObj) || currentObj == null)
            return;

        currentObj.transform.DOScale(Vector3.zero, 0.35f).OnComplete(() =>
        {
            currentObj.SetActive(false);

            idxCharacter += isLeft ? -1 : 1;

            btnNextLeft.gameObject.SetActive(idxCharacter != 0);
            btnNextRight.gameObject.SetActive(idxCharacter != UserSaveData.Ins.CharacterSaveDatas.Count - 1);

            if (objCharacters.TryGetValue(idxCharacter, out var nextObj) && nextObj != null)
            {
                nextObj.SetActive(true);
                nextObj.transform.localScale = Vector3.zero;
                nextObj.transform.DOScale(Vector3.one * 500, 0.35f);
            }

            ShowState(currSaveDatas[idxCharacter]);
        });
    }

    private void EquipOnClick()
    {
        UserSaveData.Ins.EquipCharacter(currSaveDatas[idxCharacter]);
        if (btnEquip != null)
            btnEquip.gameObject.SetActive(false);
    }

    private void UpgradeOnClick()
    {
        if (GameConfig.Ins.UpdareData == null)
            return;

        if (UserSaveData.Ins.Coin < priceUpgrade)
        {
            Hide();

            ConfirmPopup.ShowOneButtonNoQueue(
                "NOTIFY",
                "You do not have enough money to perform this action.",
                null,
                "OK");
            return;
        }

        SoundManager.Ins.PlayOneShot(SoundID.UPGRADE);

        var save = currSaveDatas[idxCharacter];
        var upgrade = GameConfig.Ins.UpdareData;

        save.CurrHP += upgrade.HP.value;
        save.CurrSpeed += upgrade.Speed.value;
        save.DamageLeft += upgrade.LATK.value;
        save.DamageRight += upgrade.RATK.value;
        save.Level++;

        UserSaveData.Ins.AddCoin(-priceUpgrade);
        UserSaveData.Ins.Save();

        ShowUpgrade(save);
    }

    private void BuyOnClick()
    {
        var save = currSaveDatas[idxCharacter];

        int priceBuy = priceUpgrade;
        if (UserSaveData.Ins.Coin < priceBuy)
        {
            Hide();

            ConfirmPopup.ShowOneButtonNoQueue(
                "NOTIFY",
                "You do not have enough money to perform this action.",
                null,
                "OK");
            return;
        }

        SoundManager.Ins.PlayOneShot(SoundID.CLICK);

        save.IsUnlock = true;
        UserSaveData.Ins.AddCoin(-priceBuy);
        UserSaveData.Ins.Save();

        ShowUpgrade(save);
    }
}
