using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] List<HealthBar> healthBarAllys;
    [SerializeField] HealthBar healthBarEnemy;
    [SerializeField] GameObject objTotalEnemy;
    [SerializeField] TextMeshProUGUI txtTotalEnemy;

    public void SetUpHelthAlly(string Name, float nomalHp, float oldHp, float curHp)
    {
        if (Name == "Player")
        {
            healthBarAllys[0].SetName(Name);
            healthBarAllys[0].SetHealth(nomalHp, oldHp, curHp);
            healthBarAllys[0].gameObject.SetActive(true);
        }
        else if (Name.Contains("1"))
        {
            healthBarAllys[1].SetName(Name);
            healthBarAllys[1].SetHealth(nomalHp, oldHp, curHp);
            healthBarAllys[1].gameObject.SetActive(true);
        }
        else {
            healthBarAllys[2].SetName(Name);
            healthBarAllys[2].SetHealth(nomalHp, oldHp, curHp);
            healthBarAllys[2].gameObject.SetActive(true);
        }
    }

    public void UpdateHelthAlly(string Name, float nomalHp, float oldHp, float curHp)
    {
        if (Name == "Player")
            healthBarAllys[0].SetHealth(nomalHp, oldHp, curHp);
        else if (Name.Contains("1"))
            healthBarAllys[1].SetHealth(nomalHp, oldHp, curHp);
        else
            healthBarAllys[2].SetHealth(nomalHp, oldHp, curHp);
    }

    public void SetUpHelthEnemy(string Name, float nomalHp, float oldHp, float curHp)
    {
        if(GameManager.Ins.GameModeEnum != GameMode.OneVSOne)
        {
            objTotalEnemy.SetActive(true);
            txtTotalEnemy.text = $"{GameManager.Ins.GameMove.Enemys.Count} Enemy";

        }
        else
            objTotalEnemy.SetActive(false);

        healthBarEnemy.SetName(Name);
        healthBarEnemy.SetHealth(nomalHp, oldHp, curHp);

    }

    public void UpdateHelthEnemy(string Name, float nomalHp, float oldHp, float curHp)
    {
        if (GameManager.Ins.GameModeEnum != GameMode.OneVSOne)
        {
            txtTotalEnemy.text = $"{GameManager.Ins.GameMove.Enemys.Count} Enemy";
        }
        healthBarEnemy.SetName(Name);
        healthBarEnemy.SetHealth(nomalHp, oldHp, curHp);
    }

    public void ReserUI()
    {
        healthBarAllys.ForEach(x => { x.gameObject.SetActive(false); });
    }
}
