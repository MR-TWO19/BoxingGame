using TwoCore;
using UnityEngine;
using Doozy.Engine.UI;
using UnityEngine.UI;

public class SettingPopup : BasePopup
{
    [SerializeField] private UIButton soundBtn, musicBtn;
    [SerializeField] private Image imgSound, imgMusic;
    [SerializeField] private Sprite SoundLight, SoundDark;

    private static SettingPopup _instance;

    public static SettingPopup Show()
    {
        if (_instance == null)
        {
            _instance = Show<SettingPopup>("SettingPopup", PopupShowMethod.QUEUE);
        }
        else
        {
            UIPopupManager.ShowPopup(_instance.UIPopup, true, false);
        }

        return _instance;
    }


    public static void HidePopup()
    {
        _instance.Hide();
    }

    private void Start()
    {
        soundBtn.OnClick.OnTrigger.Event.AddListener(BtnSoundOnClick);
        musicBtn.OnClick.OnTrigger.Event.AddListener(BtnMusicOnClick);
    }

    private void BtnSoundOnClick()
    {
        UserSaveData.Ins.SfxOn = !UserSaveData.Ins.SfxOn;
        UserSaveData.Ins.Save();
        imgSound.color = UserSaveData.Ins.SfxOn ? Color.white : Color.gray;

        if (UserSaveData.Ins.SfxOn) Soundy.ToUnmute(Soundy.SfxParam);
        else Soundy.ToMute(Soundy.SfxParam);
    }

    private void BtnMusicOnClick()
    {
        UserSaveData.Ins.MusicOn = !UserSaveData.Ins.MusicOn;
        UserSaveData.Ins.Save();
        imgMusic.color = UserSaveData.Ins.MusicOn ? Color.white : Color.gray;

        if (UserSaveData.Ins.MusicOn) Soundy.ToUnmute(Soundy.MusicParam);
        else Soundy.ToMute(Soundy.MusicParam);
    }

    protected override void OnShow()
    {
        base.OnShow();

        imgSound.color = UserSaveData.Ins.SfxOn ? Color.white : Color.gray;
        imgMusic.color = UserSaveData.Ins.MusicOn ? Color.white : Color.gray;
    }


}
