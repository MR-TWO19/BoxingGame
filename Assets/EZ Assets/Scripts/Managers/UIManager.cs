using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMono<UIManager>
{
    public UIGamePlay uiGamePlay;
    public SimpleJoystick joystick;
    public MainView mainView;
    public LevelPopup levelPopup;
    public ResultPopup resultPopup;

    private void Start()
    {
        ResetUI();
    }

    public void ShowGamePlay()
    {
        uiGamePlay.gameObject.SetActive(true);
        levelPopup.Hide();
        mainView.Hide();
    }

    public void ResetUI()
    {
        mainView.Show();
        levelPopup.Hide();
        uiGamePlay.gameObject.SetActive(false);
    }
}
