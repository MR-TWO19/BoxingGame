using DG.Tweening;
using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TwoCore;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmPopup : BasePopup
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;
    public UIButton yesButton;
    public UIButton noButton;
    public TMP_Text txtYes;
    public TMP_Text txtNo;

    public UnityAction yesAction;
    public UnityAction noAction;
    public string Title { set => titleText.text = value; }
    public string Content { set => contentText.text = value; }

    protected override void Awake()
    {
        base.Awake();
        if (yesButton)
        {
            yesButton.OnClick.OnTrigger.Event.AddListener(OnYesClick);
            yesButton.SetLabelText("YES");
        }
        if (noButton)
        {
            noButton.OnClick.OnTrigger.Event.AddListener(OnNoClick);
            noButton.SetLabelText("NO");
        }
    }

    protected override void OnShow()
    {
        base.OnShow();
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    protected override void SetParams(params object[] @params)
    {
        base.SetParams(@params);
        if (titleText && @params.Length > 0 && @params[0] != null) Title = @params[0] as string;
        if (contentText && @params.Length > 1 && @params[1] != null) Content = @params[1] as string;
        if (txtYes && @params.Length > 1 && @params[2] != null) txtYes.text = @params[2] as string;
        if (txtNo && @params.Length > 3 && @params[3] != null) txtNo.text = @params[3] as string;
    }

    public static ConfirmPopup ShowOneButtonNoQueue(string title, string content, UnityAction yes, string btnYesLabel = "Yes")
    {
        var dialog = ShowWithParamsAndMethod<ConfirmPopup>("ConfirmPopup", PopupShowMethod.NO_QUEUE, title, content, btnYesLabel);
        dialog.yesAction = yes;
        dialog.noAction = null;
        dialog.noButton.gameObject.SetActive(false);
        return dialog;
    }

    public static ConfirmPopup ShowNoQueue(string title, string content, UnityAction yes, UnityAction no, string btnYesLabel = "Yes", string btnNoLabel = "No")
    {
        var dialog = ShowWithParamsAndMethod<ConfirmPopup>("ConfirmPopup", PopupShowMethod.NO_QUEUE, title, content, btnYesLabel, btnNoLabel);
        dialog.yesAction = yes;
        dialog.noAction = no;
        dialog.noButton.gameObject.SetActive(true);
        return dialog;
    }

    public static ConfirmPopup Show(string title, string content, UnityAction yes, UnityAction no, string btnYesLabel = "Yes", string btnNoLabel = "No")
    {
        var dialog = ShowWithParams<ConfirmPopup>("ConfirmPopup", title, content, btnYesLabel, btnNoLabel);
        dialog.yesAction = yes;
        dialog.noAction = no;
        dialog.noButton.gameObject.SetActive(true);
        return dialog;
    }

    public void OnYesClick()
    {
        yesAction?.Invoke();
        Hide();
    }
    public void OnNoClick()
    {
        noAction?.Invoke();
        Hide();
    }
}