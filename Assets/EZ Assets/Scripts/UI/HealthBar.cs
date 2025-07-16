using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtName;
    [SerializeField] Image imgFill;
    [SerializeField] float duration = 0.2f;

    void Start()
    {
        imgFill.DOFillAmount(1,duration);
    }

    public void SetHealth(float nomalHp, float oldHp, float curHp)
    {
        float fill = oldHp / nomalHp;
        imgFill.DOFillAmount(fill, 0);
        float curFill = curHp / nomalHp;
        imgFill.DOFillAmount(curFill, duration);
    }

}
