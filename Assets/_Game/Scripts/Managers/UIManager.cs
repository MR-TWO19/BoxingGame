using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMono<UIManager>
{
    public InGameView uiGamePlay;
    public SimpleJoystick joystick;
    public MainView mainView;
    public PlayPopup levelPopup;
    public ResultPopup resultPopup;

    private void Start()
    {
        ResetUI();
    }

    public void ShowGamePlay()
    {
        uiGamePlay.gameObject.SetActive(true);
        levelPopup.Hide();
        //mainView.Hide();
    }

    public void ResetUI()
    {
        //mainView.Show();
        levelPopup.Hide();
        uiGamePlay.ReserUI();
        uiGamePlay.gameObject.SetActive(false);
    }
}
