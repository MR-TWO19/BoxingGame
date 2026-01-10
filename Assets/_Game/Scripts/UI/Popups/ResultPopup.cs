using Doozy.Engine.UI;
using TMPro;
using TwoCore;
using UnityEngine;

public class ResultPopup : BasePopup
{
    [SerializeField] private GameObject frameWin;
    [SerializeField] private GameObject frameLose;
    [SerializeField] private UIButton btnHomeWin;
    [SerializeField] private UIButton btnHomeLose;
    [SerializeField] private UIButton btnNextLevel;
    [SerializeField] private TextMeshProUGUI txtCoin;

    private bool _isWin;

    public static ResultPopup Show(bool isWin)
    {
        var popup = ShowWithParamsAndMethod<ResultPopup>("ResultPopup", PopupShowMethod.QUEUE, isWin);
        return popup;
    }

    protected override void SetParams(params object[] @params)
    {
        if (@params != null && @params.Length > 0 && @params[0] is bool w)
            _isWin = w;
    }

    protected override void OnShow()
    {
        if (frameWin) frameWin.SetActive(_isWin);
        if (frameLose) frameLose.SetActive(!_isWin);

        btnNextLevel.OnClick.OnTrigger.Event.AddListener(NextLevel);
        btnHomeWin.OnClick.OnTrigger.Event.AddListener(GoHome);
        btnHomeLose.OnClick.OnTrigger.Event.AddListener(GoHome);

        if(_isWin)
        {
            //txtCoin.text = $"+ {GameManager.Ins.LevelData.GoldWin}";
            //UserSaveData.Ins.AddCoin(GameManager.Ins.LevelData.GoldWin);
            UserSaveData.Ins.NextLevel();
        }
        else
        {
            //txtCoin.text = $"+ {GameManager.Ins.LevelData.GoldLose}";
            //UserSaveData.Ins.AddCoin(GameManager.Ins.LevelData.GoldLose);
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        Debug.Log("ResultPopup hidden!");
        btnNextLevel.OnClick.OnTrigger.Event.RemoveAllListeners();
        btnHomeWin.OnClick.OnTrigger.Event.RemoveAllListeners();
        btnHomeLose.OnClick.OnTrigger.Event.RemoveAllListeners();
    }


    private void NextLevel()
    {
        //GameManager.Ins.NextLevel();
        //IngameView.Ins.UpdateData();
        Hide();
    }

    private void GoHome()
    {
        //GameManager.Ins.GoHome();
        Hide();
    }
}
