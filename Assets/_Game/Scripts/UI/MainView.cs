using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    [SerializeField] Button btn1VS1;
    [SerializeField] Button btn1VSMany;
    [SerializeField] Button btnManyVSMany;

    private void Start()
    {
        btn1VS1.onClick.AddListener(OnClick1VS1);
        btn1VSMany.onClick.AddListener(OnClick1VSMany);
        btnManyVSMany.onClick.AddListener(OnClickManyVSMany);
    }

    public void Show()
   {
        gameObject.SetActive(true);
   }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClick1VS1()
    {
        UIManager.Ins.levelPopup.Show(GameMode.OneVSOne);
    }

    private void OnClick1VSMany()
    {
        UIManager.Ins.levelPopup.Show(GameMode.OneVSMany);
    }

    private void OnClickManyVSMany()
    {
        UIManager.Ins.levelPopup.Show(GameMode.ManeyVsMany);
    }
}
