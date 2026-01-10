//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class ResultPopup : MonoBehaviour
//{
//    [SerializeField] TextMeshProUGUI txtTitle;
//    [SerializeField] GameObject objWin;
//    [SerializeField] GameObject objLose;
//    [SerializeField] Button btnOk;

//    private void Start()
//    {
//        btnOk.onClick.AddListener(OnClickOK);
//    }

//    private void OnClickOK()
//    {
//        UIManager.Ins.ResetUI();
//        ObjectPoolManager.Ins.ResetItem();
//        Hide();
//    }

//    public void Show(bool isWin)
//    {
//        gameObject.SetActive(true);
//        string title = isWin ? "You Win" : "You Loso";
//        txtTitle.text = title;
//        objWin.SetActive(isWin);
//        objLose.SetActive(!isWin);

//    }

//    public void Hide()
//    {
//        gameObject.SetActive(false);
//    }
//}
