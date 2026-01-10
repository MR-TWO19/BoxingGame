using Doozy.Engine.UI;
using TwoCore;
using UnityEngine;

public class MainView : BaseView
{
    [SerializeField] private UIButton btnSetting;
    [SerializeField] private UIButton btnHowToPlay;
    [SerializeField] private UIButton btnPlay;
    [SerializeField] private UIButton btnChallenge;
    [SerializeField] private UIButton btnUpgrade;


    protected override void Awake()
    {
        base.Awake();
        btnSetting.OnClick.OnTrigger.Event.AddListener(() =>
        {
            SettingPopup.Show(true);
        });

        btnHowToPlay.OnClick.OnTrigger.Event.AddListener(() =>
        {
            HowToPlayPopup.Show();
        });
        btnPlay.OnClick.OnTrigger.Event.AddListener(() =>
        {
            PlayPopup.Show(GameMode.OneVSOne);
        });

        btnChallenge.OnClick.OnTrigger.Event.AddListener(() =>
        {
            PlayPopup.Show(GameMode.OneVSMany);
        });
        btnUpgrade.OnClick.OnTrigger.Event.AddListener(() =>
        {
            HowToPlayPopup.Show();
        });
    }

    protected override void OnShow()
    {
        base.OnShow();

        if (UserSaveData.Ins.MusicOn) Soundy.ToUnmute(Soundy.MusicParam);
        else Soundy.ToMute(Soundy.MusicParam);

        if (UserSaveData.Ins.SfxOn) Soundy.ToUnmute(Soundy.SfxParam);
        else Soundy.ToMute(Soundy.SfxParam);

        SoundManager.Ins.PlayBGMusic();
    }
}
